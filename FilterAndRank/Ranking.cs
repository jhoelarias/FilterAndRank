namespace FilterAndRank

{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Ranking
    {
        public static IList<RankedResult> FilterByCountryWithRank(
            IList<Person> people,
            IList<CountryRanking> rankingData,
            IList<string> countryFilter,
            int minRank, int maxRank,
            int maxCount)
        {
            // TODO: write your solution here.  Do not change the method signature in any way, or validation of your solution will fail.

            var result = (from person in people
                          join ranking in rankingData on person.Id equals ranking.PersonId
                          where countryFilter.Contains(ranking.Country, StringComparer.InvariantCultureIgnoreCase)
                          && ranking.Rank >= minRank && ranking.Rank <= maxRank
                          select new { person.Name, person.Id, ranking.Rank, ranking.Country });

            if (countryFilter.Any())
            {
                result = result.OrderBy(r => r.Rank)
                    .ThenBy(r => countryFilter.Select(c => c.IndexOf(r.Country)))
                    .ThenBy(r => r.Name, StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                result = result.OrderBy(r => r.Rank).ThenBy(r => r.Name, StringComparer.OrdinalIgnoreCase); ;
            }

            List<RankedResult> rankedResults = result.Select(r => new RankedResult(r.Id, r.Rank)).ToList();
            var maxRankElement = rankedResults.ElementAtOrDefault(maxCount);

            if (maxRankElement is not null)
            {
                rankedResults = rankedResults.TakeWhile(r => r.Rank <= maxRankElement.Rank).ToList();
            }

            return rankedResults;
        }
    }
}