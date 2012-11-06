﻿using NUnit.Framework;
using SearchAlgorithms.Automata;

namespace SearchAlgorithms.Test.Automata
{
    public class LevenshteinAutomataTest
    {
        private LevenshteinAutomata _automata;

        [SetUp]
        public void Initialize()
        {
            _automata = new LevenshteinAutomata("nice", 1);
        }

        [Test]
        public void NfaIsCreatedWithNoError()
        {
            Nfa nfa = _automata.Construct();
            string dotfile = nfa.WriteGraph();
            Assert.IsNotNull(nfa);
            Assert.IsNotNull(dotfile);
        }

        [Test]
        public void DfaIsCreatedWithNoError()
        {
            Dfa dfa = _automata.Construct().ConstructDfaUsingPowerSet();
            string dotfile = dfa.WriteGraph();
            Assert.IsNotNull(dfa);
            Assert.IsNotNull(dotfile);
        }

        [Test]
        public void DfaCanFindNextValidState()
        {
            Dfa dfa = _automata.Construct().ConstructDfaUsingPowerSet();
            Assert.IsNotNull(dfa);

            string next = dfa.FindNextValidString("gice");
            Assert.AreEqual(next, "gice");
        }
    }
}
