using System;
using System.Collections.Generic;
using System.Linq;

namespace AgroOrganic.Models.Recommendation
{
    public class UniqueMultipliers
    {
        private List<List<double>> combinations = new List<List<double>>();
        private List<List<double>> permutanions = new List<List<double>>();

        //added by Zhukovskyy. My simple solution for combination

        public List<List<double>> GetCombinations(int years, int addedFertilizer)
        {
            double[] arAlphabet = { 0, 2, 3, 4 };     // alphabet
            int n = years;      // number of seats in combination
            double[] arBuffer = new double[n];
            RecursionGenerateCombinations(arAlphabet, arBuffer, 0);
            //remove combinations that greater than allowed sum
            int t = 0;
            while (t < permutanions.Count)
            {
                if (permutanions[t].Sum() > addedFertilizer) permutanions.RemoveAt(t);
                else t++;
            }
            return permutanions;
        }

        void RecursionGenerateCombinations(double[] arAlphabet, double[] arBuffer, int order)
        {
            if (order < arBuffer.Length)
                for (int i = 0; i < arAlphabet.Length; i++)
                {
                    arBuffer[order] = arAlphabet[i];
                    RecursionGenerateCombinations(arAlphabet, arBuffer, order + 1);
                }
            else
            {
                List<double> list = new List<double>();
                for (int i = 0; i < arBuffer.Length; i++)
                    list.Add(arBuffer[i]);
                permutanions.Add(list);
            }
        }



        public List<List<double>> GetCombinationsOld(int year, int addedFertilizer)
        {
            var multipliers = GetAllSubSetsByArraySize(year).ToList();
            double addedFertilizerPerYear = (double)addedFertilizer / year;

            for (int i = 0; i < multipliers.Count; i++)
            {
                for (int j = 0; j < multipliers[i].Count; j++)
                {
                    multipliers[i][j] *= addedFertilizerPerYear;
                }
            }

            return multipliers;
        }

        public List<List<double>> GetAllSubSetsByArraySize(int size)
        {
            GetAllSubSets(size, size, "");

            foreach (var i in combinations)
            {
                foreach (double[] permutation in Permutations<double>.AllFor(i.ToArray()))
                {
                    permutanions.Add(permutation.ToList());
                }
            }

            return permutanions.Distinct(new ArrayComparer()).ToList();
        }

        public void GetAllSubSets(int N, int curr, String res)
        {
            var zeros = new double[N];
            zeros.Initialize();

            if (curr == 0)
            {
                var list = new List<double>(zeros);
                var combination = res.ToCharArray().Select(x => Convert.ToDouble(x.ToString())).ToList();

                for (var i = 0; i < combination.Count; i++)
                {
                    list[i] = combination[i];
                }

                combinations.Add(list);
                return;
            }

            for (int i = 1; i <= N; i++)
            {
                if (i <= curr)
                {
                    GetAllSubSets(N, curr - i, res + i);
                }
            }
        }
    }

    class ArrayComparer : IEqualityComparer<List<double>>
    {
        public bool Equals(List<double> x, List<double> y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            if (x.Count != y.Count)
                return false;

            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] != y[i])
                    return false;
            }

            return true;
        }

        public int GetHashCode(List<double> intArray)
        {
            if (ReferenceEquals(intArray, null)) return 0;

            int hashCode = 0;
            bool isFirst = true;
            foreach (int i in intArray)
            {
                if (isFirst)
                {
                    hashCode = i;
                    isFirst = false;
                }
                else
                {
                    hashCode = hashCode ^ i;
                }
            }
            return hashCode;
        }
    }

    class Permutations<T>
    {
        public static IEnumerable<T[]> AllFor(T[] array)
        {
            if (array == null || array.Length == 0)
            {
                yield return new T[0];
            }
            else
            {
                for (int pick = 0; pick < array.Length; ++pick)
                {
                    T item = array[pick];
                    int i = -1;
                    T[] rest = Array.FindAll(
                        array, delegate (T p) { return ++i != pick; }
                    );
                    foreach (T[] restPermuted in AllFor(rest))
                    {
                        i = -1;
                        yield return Array.ConvertAll(
                            array,
                            delegate (T p)
                            {
                                return ++i == 0 ? item : restPermuted[i - 1];
                            }
                        );
                    }
                }
            }
        }
    }
}