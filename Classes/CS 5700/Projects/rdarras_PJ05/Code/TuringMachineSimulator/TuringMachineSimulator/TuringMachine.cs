using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuringMachineSimulator
{
    class TuringMachine
    {
        public List<char> Tape { get; set; } = new List<char>();
        public int TapeHeadLocation { get; set; } = 0;
        public List<int> States { get; private set; } = new List<int>();
        public List<char> Alphabet { get; private set; } = new List<char>(); //normal printing characters
        public List<char> TapeAlphabet { get; private set; } = new List<char>();
        public Dictionary<int, List<Transition>> StateTransitions { get; private set; } = new Dictionary<int, List<Transition>>();
        public int CurrentState { get; private set; } = 0;
        public int AcceptState { get; private set; } = 254;
        public int RejectState { get; private set; } = 255;

        /// <summary>
        /// Constructor for the turing machine
        /// </summary>
        /// <param name="transitions">each line read from the file</param>
        public TuringMachine(List<string> transitions)
        {
            List<Transition> allTransitions = new List<Transition>();

            //For each transition in the machine file
            foreach (string s in transitions)
            {
                string[] parsedString = s.Split(',');

                //Handle any errors
                if (parsedString.Length != 5)
                    continue;

                //Parse out the strings data into the following fields
                int from = int.Parse(parsedString[0]);
                char read = (char)int.Parse(parsedString[1]);
                int to = int.Parse(parsedString[2]);
                char write = (char)int.Parse(parsedString[3]);
                char direction = char.Parse(parsedString[4]);

                //Handle the states and the tape alphabet
                if (!States.Contains(from))
                    States.Add(from);
                if (!States.Contains(to))
                    States.Add(to);
                if (!TapeAlphabet.Contains(read))
                    TapeAlphabet.Add(read);
                if (!TapeAlphabet.Contains(write))
                    TapeAlphabet.Add(write);
                if (!TapeAlphabet.Contains((char)0))
                    TapeAlphabet.Add((char)0);

                //add the transition data to the transitions list
                allTransitions.Add(new Transition(from, read, to, write, direction));
            }

            //for each state, create a list of transitions from it
            foreach (int i in States)
            {
                StateTransitions.Add(i, new List<Transition>());
            }

            //for each transition, put it in the appropriate spot in StateTransitions
            foreach (Transition t in allTransitions)
            {
                StateTransitions[t.FromState].Add(new Transition(t));
            }
        }

        /// <summary>
        /// Runs the machine on the specified input string
        /// </summary>
        /// <param name="input">the input</param>
        /// <returns>the output</returns>
        public string RunMachineOnInput(string input)
        {
            Tape.Clear();
            TapeHeadLocation = 0;
            CurrentState = 0;

            //Make the tape match the input
            foreach (char c in input)
            {
                Tape.Add(c);
            }

            if (input.Length == 0)
            {
                Tape.Add('0');
            }

            //Run indefinately assuming our input isn't the empty string.
            while(input.Length > 0)
            {
                //Store the transition that we want to use
                Transition t = StateTransitions[CurrentState].FirstOrDefault(y => y.ReadSymbol == Tape[TapeHeadLocation]);

                //If a valid transition exists
                if (t != null)
                {
                    //Update our information to reflect our new global state (global state = setting of all values in the machine)
                    CurrentState = t.ToState;
                    Tape[TapeHeadLocation] = t.WriteSymbol;
                    TapeHeadLocation = t.HeadDirection == 'R' ? TapeHeadLocation + 1 : TapeHeadLocation != 0 ? TapeHeadLocation - 1 : 0;

                    //Did we go too far right? if so, add the blank character
                    if (TapeHeadLocation == Tape.Count)
                        Tape.Add((char)0);

                    //did we land on a halting state? if so, break.
                    if (CurrentState == AcceptState || CurrentState == RejectState)
                    {
                        break;
                    }
                }
                else
                {
                    //we ran into a point where we can't make a transition
                    break;
                }
            }


            //reset our tools and return the output
            string output = "";
            foreach (char c in Tape)
            {
                output += c;
            }
            while(true)
            {
                if (output.Length < 2)
                {
                    break;
                }
                if (output[output.Length - 1] == (char)0)
                {
                    output = output.Substring(0, output.Length - 1);
                }
                else
                {
                    break;
                }
            }
            return output;
        }
    }


    /// <summary>
    /// A transition
    /// </summary>
    class Transition
    {
        public int FromState { get; private set; }
        public char ReadSymbol { get; private set; }
        public int ToState { get; private set; }
        public char WriteSymbol { get; private set; }
        public char HeadDirection { get; private set; }

        public Transition(int fromState, char readSymbol, int toState, char writeSymbol, char headDirection)
        {
            FromState = fromState;
            ReadSymbol = readSymbol;
            ToState = toState;
            WriteSymbol = writeSymbol;
            HeadDirection = headDirection;
        }

        public Transition(Transition t)
        {
            FromState = t.FromState;
            ReadSymbol = t.ReadSymbol;
            ToState = t.ToState;
            WriteSymbol = t.WriteSymbol;
            HeadDirection = t.HeadDirection;
        }
    }
}
