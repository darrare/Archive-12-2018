using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteAutomatonSimulator
{
    /// <summary>
    /// The possible types of Finite Automata
    /// </summary>
    public enum FiniteAutomatonType { DFA, NFA, INVALID }

    class FiniteAutomatonManager
    {

        #region Singleton

        static FiniteAutomatonManager instance;
        public static FiniteAutomatonManager Instance => instance ?? (instance = new FiniteAutomatonManager());

        #endregion

        #region Constructor

        /// <summary>
        /// Private constructor for singleton FiniteAutomatonManager
        /// </summary>
        private FiniteAutomatonManager() { }

        #endregion

        #region Properties

        public NonDeterministicFiniteAutomaton CurrentMachine { get; private set; }
        public List<NonDeterministicFiniteAutomaton> FiniteAutomatons { get; private set; } = new List<NonDeterministicFiniteAutomaton>();
        public string CurrentTotalString { get; private set; }
        public string CurrentRemainingString { get; private set; }
        public List<string> InputStrings { get; private set; } = new List<string>();
        public List<string> AcceptedStrings { get; private set; } = new List<string>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a Finite Automata from the input strings that were read from a file
        /// </summary>
        /// <param name="strings">The strings read from the file, that we will use to create a Finite Automata</param>
        /// <returns>The type of machine this is. DFA, NFA, or Invalid</returns>
        public FiniteAutomatonType CreateFiniteAutomataFromStrings(List<string> strings)
        {
            List<int> states = new List<int>();
            List<char> alphabet = new List<char>();
            List<Transition> transitions = new List<Transition>();
            List<int> acceptStates = ParseAcceptStateString(strings[0]);

            string[] data = new string[3];
            for (int i = 1; i < strings.Count; i++)
            {
                //Split the string into the correct data. (TODO: Might need to handle EOL or EOF stuff.)
                data = strings[i].Split(',');

                //Error handle end of file stuff (or any other wrong input)
                if (data.Length != 3)
                {
                    continue;
                }

                //Store data into correct data types (to prevent from calling int/char .parse an unnecesary amount of times.)
                int a = int.Parse(data[0]);
                int b = int.Parse(data[2]);
                char c = char.Parse(data[1]);

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
            CurrentMachine = new NonDeterministicFiniteAutomaton(states, alphabet, transitions, acceptStates, 0);
            FiniteAutomatons.Clear();
            FiniteAutomatons.Add(CurrentMachine);
            FiniteAutomatons = FiniteAutomatons.Union(CurrentMachine.GetEpsilonTransitionedMachines()).ToList();

            return CurrentMachine.GetMachineType();
        }

        /// <summary>
        /// Runs a set of strings against the current machine. 
        /// </summary>
        /// <param name="strings">The set of strings to run</param>
        /// <returns>The list of accepted strings</returns>
        public List<string> TestFiniteAutomataAgainstStrings(List<string> strings)
        {
            List<string> acceptedStrings = new List<string>();
            InputStrings = strings;

            foreach (string s in strings)
            {
                List<NonDeterministicFiniteAutomaton> toBe = new List<NonDeterministicFiniteAutomaton>();
                foreach (char c in s)
                {
                    toBe.Clear();
                    foreach (NonDeterministicFiniteAutomaton FA in FiniteAutomatons)
                    {
                        toBe = toBe.Union(FA.Advance(c)).ToList();
                    }
                    FiniteAutomatons = new List<NonDeterministicFiniteAutomaton>(toBe);
                }

                if (toBe.Any(t => t.AcceptStates.Contains(t.CurState)))
                {
                    acceptedStrings.Add(s);
                }

                FiniteAutomatons.Clear();
                FiniteAutomatons.Add(CurrentMachine);
                FiniteAutomatons = FiniteAutomatons.Union(CurrentMachine.GetEpsilonTransitionedMachines()).ToList();
            }

            AcceptedStrings = acceptedStrings;
            return acceptedStrings;
        }

        /// <summary>
        /// Gets the data required for the log file.
        /// </summary>
        /// <returns>The list of strings to turn into the log file.</returns>
        public List<string> GetLogDataFromCurrentMachine()
        {
            List<string> lines = new List<string>();
            lines.Add("Valid: " + CurrentMachine.GetMachineType().ToString());
            lines.Add("States: " + CurrentMachine.States.Count());
            lines.Add("Alphabet: " + CurrentMachine.GetAlphabetInOrder());
            lines.Add("Accepted Strings: " + AcceptedStrings.Count + "/" + InputStrings.Count);

            return lines;
        }

        #endregion

        #region Private Methods

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

        #endregion
    }
}
