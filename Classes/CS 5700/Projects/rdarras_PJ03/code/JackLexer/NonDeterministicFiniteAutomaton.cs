using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace JackLexer
{

    /// <summary>
    /// The possible types of Finite Automata
    /// </summary>
    public enum FiniteAutomatonType { DFA, NFA, INVALID }

    [Serializable]
    class NonDeterministicFiniteAutomaton
    {
        public List<int> States { get; private set; } = new List<int>();
        public List<char> Alphabet { get; private set; } = new List<char>();
        public Dictionary<int, List<Transition>> StateTransitions { get; private set; } = new Dictionary<int, List<Transition>>();
        public int StartState { get; private set; } = 0;
        public List<int> AcceptStates { get; private set; } = new List<int>();

        public int CurState { get; set; }

        /// <summary>
        /// Constructor for an NFA
        /// </summary>
        /// <param name="states">List of states in the machine Q</param>
        /// <param name="alphabet">The alphabet of the machine Σ</param>
        /// <param name="transitions">List of the transitions in the machine δ</param>
        /// <param name="acceptStates">List of accept states in the machine F</param>
        /// <param name="curState">Current state of the machine, used for when making copies</param>
        public NonDeterministicFiniteAutomaton(List<int> states, List<char> alphabet, List<Transition> transitions, List<int> acceptStates, int curState = 0)
        {
            States = states;
            Alphabet = alphabet;
            AcceptStates = acceptStates;
            CurState = curState;

            //For each state, add it to the StateTransitions dictionary
            foreach (int i in states)
            {
                StateTransitions.Add(i, new List<Transition>());
            }

            foreach (Transition t in transitions)
            {
                StateTransitions[t.FromState].Add(new Transition(t));
            }
        }

        /// <summary>
        /// Copy constructor for NFA
        /// </summary>
        /// <param name="c">The machine we want to copy</param>
        public NonDeterministicFiniteAutomaton(NonDeterministicFiniteAutomaton c)
        {
            States = new List<int>(c.States);
            Alphabet = new List<char>(c.Alphabet);
            AcceptStates = new List<int>(c.AcceptStates);
            StateTransitions = new Dictionary<int, List<Transition>>(c.StateTransitions);
            CurState = c.CurState;
        }

        /// <summary>
        /// Advance this machine in every possible way it can go based on the input character.
        /// </summary>
        /// <param name="inputChar">The input character</param>
        /// <returns>The list of new machines that are a result of this one advancing a stage.</returns>
        public List<NonDeterministicFiniteAutomaton> Advance(char inputChar)
        {
            List<NonDeterministicFiniteAutomaton> newMachines = new List<NonDeterministicFiniteAutomaton>();
            foreach (Transition t in GetTransitionsFromCurrentStateWithInputChar(inputChar))
            {
                newMachines.Add(new NonDeterministicFiniteAutomaton(this));
                newMachines.Last().CurState = t.ToState;
            }

            newMachines = newMachines.Union(GetEpsilonTransitionedMachines()).ToList();

            return newMachines;
        }

        /// <summary>
        /// Gets a list of any epsilon transitions from this machine so that we can also process them on this stage of the string.
        /// </summary>
        /// <returns></returns>
        public List<NonDeterministicFiniteAutomaton> GetEpsilonTransitionedMachines()
        {
            List<NonDeterministicFiniteAutomaton> newMachines = new List<NonDeterministicFiniteAutomaton>();
            foreach (Transition t in GetTransitionsFromCurrentStateWithEpsilon())
            {
                newMachines.Add(new NonDeterministicFiniteAutomaton(this));
                newMachines.Last().CurState = t.ToState;
            }

            foreach (NonDeterministicFiniteAutomaton machine in newMachines)
            {
                newMachines = newMachines.Union(machine.GetEpsilonTransitionedMachines()).ToList();
            }
            return newMachines;
        }

        /// <summary>
        /// Does this machine have a transition from the current state that says if input = 'x' go to multiple places OR has transition for input '`'
        /// </summary>
        /// <returns>If this machine should be duplicated at this point</returns>
        bool HasNFATransitionsInCurrentState()
        {
            //If has epsilon transition
            if (StateTransitions[CurState].Any(t => t.InputChar == '`'))
            {
                return true;
            }
            //If has multiple transitions with same input
            if (StateTransitions[CurState].GroupBy(x => x.InputChar).Any(t => t.Count() > 1))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the transitions currently available for this machine
        /// </summary>
        /// <returns></returns>
        List<Transition> GetTransitionsFromCurrentState()
        {
            return StateTransitions[CurState];
        }

        /// <summary>
        /// Gets all transitions for this machine that have input char of inputChar
        /// </summary>
        /// <param name="inputChar">the input char</param>
        /// <returns>the list of transitions</returns>
        List<Transition> GetTransitionsFromCurrentStateWithInputChar(char inputChar)
        {
            return StateTransitions[CurState].Where(t => t.InputChar == inputChar).ToList();
        }

        /// <summary>
        /// Gets a list of epsilon transitions
        /// </summary>
        /// <returns>The list of epsilon transitions</returns>
        List<Transition> GetTransitionsFromCurrentStateWithEpsilon()
        {
            return new List<Transition>(); //StateTransitions[CurState].Where(t => t.InputChar == '`').ToList();
        }

        /// <summary>
        /// Gets all transitions for this machine that have input char of inputChar or epsilon
        /// </summary>
        /// <param name="inputChar">the input char</param>
        /// <returns>the list of transitions</returns>
        List<Transition> GetTransitionsFromCurrentStateWithInputCharOrEpsilon(int inputChar)
        {
            return StateTransitions[CurState].Where(t => t.InputChar == inputChar || t.InputChar == '`').ToList();
        }

        /// <summary>
        /// Gets the list of transitions that are considered to be NFA
        /// </summary>
        /// <returns>The list of transitions that are considered to be NFA</returns>
        List<Transition> GetNFATransitionsFromCurrentState()
        {
            List<Transition> t = new List<Transition>();
            t = t.Union(StateTransitions[CurState].Where(x => x.InputChar == '`')).ToList();
            t = t.Union(StateTransitions[CurState].GroupBy(x => x.InputChar).Where(y => y.Count() > 1).SelectMany(group => group)).ToList();
            return t;
        }

        /// <summary>
        /// Gets the type of the machine. DFA, NFA, or Invalid
        /// </summary>
        /// <returns>The type of the machine.</returns>
        public FiniteAutomatonType GetMachineType()
        {
            //First, check machines validity
            //Are all states labeled between 0 and 255?
            if (States.Any(t => t < 0 || t > 255))
                return FiniteAutomatonType.INVALID;
            //Is the alphabet comprised of only printable characters? Ref: https://www.juniper.net/documentation/en_US/idp5.1/topics/reference/general/intrusion-detection-prevention-custom-attack-object-extended-ascii.html
            if (Alphabet.Any(t => (int)t < 32))
                return FiniteAutomatonType.INVALID;
            /*Third check as per assignment description states: "Third, the transition rules should only be defined from and to valid states."
              The way I have designed this program, the first check will result in a failure in the case that any transition is defined to an invalid state.*/

            //Is this a DFA or an NFA?
            //If it has the "epsilon" character, '`' for the purpose of this assignment, it is an NFA
            if (StateTransitions.Any(t => t.Value.Any(i => i.InputChar == '`')))
                return FiniteAutomatonType.NFA;

            //If two transitions have the same FromState and InputChar, it is an NFA
            if (StateTransitions.Any(i => i.Value.GroupBy(x => x.InputChar).Any(t => t.Count() > 1)))
                return FiniteAutomatonType.NFA;

            //Otherwise, it is a DFA
            return FiniteAutomatonType.DFA;
        }

        /// <summary>
        /// Sorts and returns this machines alphabet.
        /// </summary>
        /// <returns></returns>
        public string GetAlphabetInOrder()
        {
            Alphabet.Sort();
            string sorted = "";
            foreach (char s in Alphabet)
            {
                sorted += s;
            }
            return sorted;
        }
    }

    /// <summary>
    /// A transition is a custom class that represents a transiton from State X to State Y via Input Z
    /// </summary>
    [Serializable]
    class Transition
    {
        public int FromState { get; private set; }
        public int ToState { get; private set; }
        public char InputChar { get; private set; }

        public Transition(int from, int to, char input)
        {
            FromState = from;
            ToState = to;
            InputChar = input;
        }

        public Transition(Transition t)
        {
            FromState = t.FromState;
            ToState = t.ToState;
            InputChar = t.InputChar;
        }
    }
}
