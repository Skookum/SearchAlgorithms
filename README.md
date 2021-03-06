# SearchAlgorithms

## Overview

Project that defines a .NET search algorithms module. It currently supports the following:

+ String search through large dictionaries using Levenshtein Automata

The project will generate a Nuget package that can be downloaded as a 3rd party dll via Visual Studio.

## Example

````csharp
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SearchAlgorithms.StringStrategies;

namespace SearchAlgorithms.Test.SearchStrategies
{
    public class LevenshteinAutomataStringStrategyTest
    {
        private IList<string> _dataset;
        private List<string> _largeDataset;
        private LevenshteinAutomataStringStrategy _strategy;
        private SortedList _sortedDataSet;

        [SetUp]
        public void Initialize()
        {
            _strategy = new LevenshteinAutomataStringStrategy();
            _SetupDataset();
            _SetupLargeDataset();
        }

        [Test]
        public void ShouldReturnStringsThatAreEqual()
        {
            IEnumerable<string> results = _strategy.Search("nice", _dataset);
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void ShouldReturnStringsThatAreOnlyOffByAtMostOneCharacter()
        {
            IEnumerable<string> results = _strategy.Search("nice", _dataset.OrderBy(x => x).ToList(), 1);
            Assert.AreEqual(3, results.Count());
        }

        [Test]
        public void ShouldReturnStringsThatAreOnlyOffByAtMostTwoCharacters()
        {
            IEnumerable<string> results = _strategy.Search("nice", _dataset.OrderBy(x => x).ToList(), 2);
            Assert.AreEqual(5, results.Count());
        }

        [Test]
        public void ShouldReturnStringsFromLargeDatasetThatAreEqual()
        {
            IEnumerable<string> results = _strategy.Search("elephant", _largeDataset);
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void ShouldReturnStringsFromLargeDatasetThatAreNotEqual()
        {
            string[] dataset = new string[_sortedDataSet.Count];
            _sortedDataSet.Values.CopyTo(dataset, 0);
            IEnumerable<string> results = _strategy.Search("Randy".ToLower(), dataset.ToList(), 1);

            List<string> finalResults = new List<string>();
            foreach (string item in _largeDataset)
            {
                foreach (string result in results)
                    if (item.ToLower().Contains(result))
                        finalResults.Add(item);
            }

            Assert.AreEqual(527, finalResults.Count);
        }

        private void _SetupDataset()
        {
            _dataset = new List<string>
                {
                    "applepie",
                    "nice",
                    "rice",
                    "niceso",
                    "plepie",
                    "applep",
                    "genes",
                    "beer",
                    "nici",
                    "wizened",
                    "great",
                    "bait",
                    "sonice",
                    "mate"
                };
        }

        private void _SetupLargeDataset()
        {
            _largeDataset = new List<string>();
            HashSet<SearchResult> searchResults = new HashSet<SearchResult>();
            StreamReader reader = new StreamReader(@"results.csv");
            reader.ReadLine(); //do not process header

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] entries = line.Split(new [] {','}, System.StringSplitOptions.None);
                SearchResult sr = new SearchResult(entries[0]);
                searchResults.Add(sr);
            }

            _largeDataset.AddRange(searchResults.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name));
            List<string> intermediateDataSet = new List<string>();
            intermediateDataSet.AddRange(_largeDataset.SelectMany(x => x.Split(new[] {' '})));

            _sortedDataSet = new SortedList(
                intermediateDataSet.Where(x => !string.IsNullOrWhiteSpace(x)).
                                    Select(x => x.ToLower().Trim()).
                                    Distinct().
                                    ToDictionary(str => str));
        }
    }
}
````

## License

This code is released under the [MIT license](http://opensource.org/licenses/MIT) with no guarantees.