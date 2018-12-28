using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLexer
{
    /// <summary>
    /// These are the types of DFA's that look at the string at any given time period
    /// </summary>
    public enum NFAType
    {
        KW_CONST,
        KW_TYPE,
        KW_VARDEC,
        KW_SUBDEC,
        KW_VAR,
        KW_VOID,
        KW_CLASS,
        KW_LET,
        KW_IF,
        KW_ELSE,
        KW_WHILE,   
        KW_DO,
        KW_RETURN,
        SY_LPAREN,
        SY_RPAREN,
        SY_LBRACKET,
        SY_RBRACKET,
        SY_LBRACE,
        SY_RBRACE,
        SY_SEMI,
        SY_PERIOD,
        SY_COMMA,
        SY_EQ,
        SY_MINUS,
        SY_NOT,
        SY_OP,
        IDENT,
        INTEGER,
        STRING,
        COMMENT_EOL,
        COMMENT_BLK,
        EOL,
        WHITESPACE
    }

    class JackTokenizer
    {
        public string GlobalStage { get; set; }
        public string IndividualStage { get; set; }

        /// <summary>
        /// Baseline machines that don't really do anything, they are just there to copy from.
        /// </summary>
        public Dictionary<NFAType, NonDeterministicFiniteAutomaton> BaseMachines { get; private set; } = new Dictionary<NFAType, NonDeterministicFiniteAutomaton>();

        /// <summary>
        /// List of machines because of the potential of an NFA. In the case that we only use DFA's, then the count of the list will always be 1
        /// </summary>
        public Dictionary<NFAType, List<NonDeterministicFiniteAutomaton>> FiniteAutomatons { get; private set; } = new Dictionary<NFAType, List<NonDeterministicFiniteAutomaton>>();

        /// <summary>
        /// A tracker to see what dfa found the largest match.
        /// </summary>
        public Dictionary<NFAType, int> LongestSegment { get; private set; } = new Dictionary<NFAType, int>();

        /// <summary>
        /// Constructor for the jack tokenizer
        /// </summary>
        public JackTokenizer(string jackFile)
        {
            GlobalStage = jackFile;

            foreach (NFAType t in Enum.GetValues(typeof(NFAType)))
            {
                CreateFiniteAutomataFromStrings(ConvertFileToListOfStrings(t.ToString() + ".fa"), t);
            }
            ResetMachines();
        }


        /// <summary>
        /// Creates a Finite Automata from the input strings that were read from a file
        /// </summary>
        /// <param name="strings">The strings read from the file, that we will use to create a Finite Automata</param>
        /// <returns>The type of machine this is. DFA, NFA, or Invalid</returns>
        void CreateFiniteAutomataFromStrings(List<string> strings, NFAType type)
        {
            List<int> states = new List<int>();
            List<char> alphabet = new List<char>();
            List<Transition> transitions = new List<Transition>();
            List<int> acceptStates = ParseAcceptStateString(strings[0]);
            int a = 0, b = 0;
            char c = '_';

            string[] data = new string[3];
            for (int i = 1; i < strings.Count; i++)
            {
                //Split the string into the correct data. (TODO: Might need to handle EOL or EOF stuff.)
                data = strings[i].Split(',');

                if (data.Length == 3)
                {
                    //Store data into correct data types (to prevent from calling int/char .parse an unnecesary amount of times.)
                    a = int.Parse(data[0]);
                    b = int.Parse(data[2]);

                    if (data[1] == "\\r")
                        c = '\r';
                    else if (data[1] == "\\n")
                        c = '\n';
                    else if (data[1] == "\\t")
                        c = '\t';
                    else
                        c = char.Parse(data[1]);
                }
                else if (data.Length == 4)
                {
                    //Store data into correct data types (to prevent from calling int/char .parse an unnecesary amount of times.)
                    a = int.Parse(data[0]);
                    b = int.Parse(data[3]);
                    c = ',';
                }

                //Check to see if our DFA/NFA already contains these elements, if not add them
                if (!states.Contains(a))
                    states.Add(a);
                if (!states.Contains(b))
                    states.Add(b);
                if (!alphabet.Contains(c))
                    alphabet.Add(c);

                //Add the transition to the list
                transitions.Add(new Transition(a, b, c));

            }

            //Set our managers current machine to be comprised of our new data and push it to our list of running machines
            BaseMachines.Add(type, new NonDeterministicFiniteAutomaton(states, alphabet, transitions, acceptStates, 0));
            FiniteAutomatons.Add(type, new List<NonDeterministicFiniteAutomaton>());
            LongestSegment.Add(type, 0);
        }

        /// <summary>
        /// Resets the dictionary of lists to its base value so that we can iterate upon the string again.
        /// </summary>
        void ResetMachines()
        {
            //Reset each list of machines to the baseline so we can start on the next part of our global string.
            foreach (NFAType t in Enum.GetValues(typeof(NFAType)))
            {
                FiniteAutomatons[t].Clear();
                FiniteAutomatons[t].Add(BaseMachines[t]);
                FiniteAutomatons[t].Union(BaseMachines[t].GetEpsilonTransitionedMachines());
                LongestSegment[t] = 0;
            }
        }

        /// <summary>
        /// Converts the jackFile into a list of strings (tokens)
        /// </summary>
        /// <param name="jackFile">The stream read jack file</param>
        /// <returns>The list of tokens</returns>
        public List<string> TokenizeString()
        {
            List<string> tokens = new List<string>();

            //Lord have mercy on me
            //While we still have data to parse
            while(GlobalStage.Length > 0)
            {
                //for each collection of machines related to each NFAType (in case we use "NFA" features such as spawning off another machine)
                foreach (KeyValuePair<NFAType, List<NonDeterministicFiniteAutomaton>> col in FiniteAutomatons)
                {
                    List<NonDeterministicFiniteAutomaton> CurrentList = col.Value;
                    IndividualStage = GlobalStage;
                    int count = 0;
                    //While our string can continue to parse through the specific NFAType machine. When a machine can't run anymore, it gets removed.
                    while (CurrentList.Count > 0)
                    {
                        List<NonDeterministicFiniteAutomaton> toBe = new List<NonDeterministicFiniteAutomaton>();
                        //For every nfa of this type
                        foreach (NonDeterministicFiniteAutomaton nfa in CurrentList)
                        {
                            //Check to see if it is on an accept state, if it is store this value as the most recent accept state event.
                            if (nfa.AcceptStates.Contains(nfa.CurState))
                                LongestSegment[col.Key] = count;
                            //We are at the end of the string, therefore we cannot progress our machine any further. This could probably be break instead of continue, but I'm not worried about performance right now.
                            if (IndividualStage.Length == 0)
                                continue;
                            //Start building up the list of machines to use on the next input character
                            toBe = toBe.Union(nfa.Advance(IndividualStage[0])).ToList();
                        }
                        //Set the current list to what we have compiled for the current input character. This contains all possible transitions (in the case of DFA, CurrentList.Count will be 1)
                        CurrentList = new List<NonDeterministicFiniteAutomaton>(toBe);
                        //Error handling. Can't remove from an empty string.
                        if (IndividualStage.Length != 0)
                            IndividualStage = IndividualStage.Remove(0, 1);
                        count++;
                    }
                }
                //Store the data of the longest value "Max Munch Principle". 
                KeyValuePair<NFAType, int> pair = LongestSegment.Aggregate((x, y) => x.Value >= y.Value ? x : y);

                //we don't want to add some types to our list as per the instructions.
                if (pair.Key != NFAType.EOL && pair.Key != NFAType.WHITESPACE && pair.Key != NFAType.COMMENT_BLK && pair.Key != NFAType.COMMENT_EOL)
                    tokens.Add(pair.Key.ToString() + ", " + GlobalStage.Substring(0, pair.Value));

                //Skim the current tokenized line from the global stage string so that we can process the next step of it.
                GlobalStage = GlobalStage.Remove(0, pair.Value);
                ResetMachines();
            }
            return tokens;
        }

        /// <summary>
        /// Parses the {x,y,...,z} string that declares the accept states
        /// </summary>
        /// <param name="s">the curly brace enclosed, comma separated list of accept states</param>
        /// <returns>The list of accept states.</returns>
        List<int> ParseAcceptStateString(string s)
        {
            //Simple linq statement. Delimits the string by , { and } and then converts it into a List<int>
            return s.Split(new char[] { ',', '{', '}' }).Where(t => !string.IsNullOrEmpty(t)).Select(int.Parse).ToList();
        }

        /// <summary>
        /// Opens the machine file and converts it to a list of strings that we can parse through.
        /// </summary>
        /// <param name="fileName">Name of the file. App path will be appended (app path will be location of binary)</param>
        /// <returns>The new list of strings</returns>
        static List<string> ConvertFileToListOfStrings(string fileName)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory + "Machines\\" + fileName;
            List<string> strings = new List<string>();

            using (StreamReader sr = new StreamReader(appPath))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    strings.Add(line);
                }
            }
            return strings;
        }
    }
}
