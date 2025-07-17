using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Extensions
{
    public static class ListExtensions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            var min = 0;
            var max = list.Count;

            var rnd = Random.Range(min, max);
            
            return list[rnd];
        }
    }
}