using UnityEngine;
using System.Collections;
//This class is non-textbook implementation
//public static class QMethod
//{
//    public static void Log(string s)
//    {
//        Debug.Log(s);
//    }
//    //
//    ////public static readonly CultureInfo CultureEnUs = new CultureInfo("en-US");
//    //
//    //public static string ToStringEnUs(this double d)
//    //{
//    //    return d.ToString("G", CultureEnUs);
//    //}
//    //public static string Pretty(this double d)
//    //{
//    //    return ToStringEnUs(Mathf.Round((float)d));
//    //}
//    //
//    public static string ActionNameFromTo(string a, string b)
//    {
//        return string.Format("from_{0}_to_{1}", a, b);
//    }
//    //
//    //public static string EnumToString<T>(this T type)
//    //{
//    //    return Enum.GetName(typeof(T), type);
//    //}
//    //
//    //public static void ValidateRange(double d, string origin = null)
//    //{
//    //    if (d < 0 || d > 1)
//    //    {
//    //        string s = origin ?? string.Empty;
//    //        throw new ApplicationException(string.Format("ValidateRange error: {0} {1}", d, s));
//    //    }
//    //}

//    public static void Validate(QLearning q)
//    {
//        foreach (QState state in q.States)
//        {
//            foreach (QAction action in state.actions)
//            {
//                action.ValidateActionsResultProbability();
//            }
//        }
//    }
//}
