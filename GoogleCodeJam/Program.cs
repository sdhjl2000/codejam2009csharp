using System;
using System.Collections.Generic;
using System.IO;
public class MyClass
{
    #region  A
    public static void GetCaseA()
    {
        string m_filepath = @"e:\data.in";
        int l = 0;//字符串长度
        int d = 0;//单词个数
        int n = 0;//模式个数

        using (StreamReader sr = new StreamReader(m_filepath))
        {

            string firstline = sr.ReadLine();
            string[] fline = firstline.Split(' ');
            l = int.Parse(fline[0]);
            d = int.Parse(fline[1]);
            n = int.Parse(fline[2]);
            Console.Write(string.Format("{0}字符,{1}个单词,{2}个匹配模式\n", l, d, n));
            char[,] allword = new char[d, l];//定义包含所有单词的数组
            for (int i = 0; i < d; i++)
            {
                char[] temparry = sr.ReadLine().ToCharArray();
                for (int j = 0; j < l; j++)
                {
                    allword[i, j] = temparry[j];
                }
                // Console.Write(allword[i,0]+"\n");
            }
            string[,] allpattern = new String[n, l];
            //读取
            for (int p = 0; p < n; p++)
            {
                string pline = sr.ReadLine();
                GetPattern(ref allpattern, p, l, pline, 0);
            }
            int[] casenum = new int[n];//记录匹配数

            for (int mm = 0; mm < n; mm++)
            {
                for (int m = 0; m < d; m++)
                {
                    bool match = true;
                    for (int mmm = 0; mmm < l; mmm++)
                    {
                        if (allpattern[mm, mmm].IndexOf(allword[m, mmm]) == -1)
                        { match = false; break; }
                    }
                    if (match)
                    {
                        casenum[mm]++;
                    }
                }
            }
            for (int dd = 0; dd < n; dd++)
            {
                Console.Write("Case #{0}:{1}\n", dd, casenum[dd]);
            }
            using (StreamWriter sw = new StreamWriter(@"e:\out.txt"))
            {
                for (int dd = 0; dd < n; dd++)
                {
                    sw.WriteLine(string.Format("Case #{0}: {1}\n", dd + 1, casenum[dd]));
                }
            }

        }
    }
    public static void GetPattern(ref string[,] allpattern, int linenum, int strnum, string line, int start)
    {
        int n = line.IndexOf('(');
        for (int i = 0; i < n; i++)
        {
            allpattern[linenum, start++] = line[i].ToString();
        }
        if (n >= 0)
        {
            int l = line.IndexOf(')');
            allpattern[linenum, start++] = line.Substring(n + 1, l - 1 - n);
            GetPattern(ref allpattern, linenum, strnum, line.Substring(l + 1), start);
        }
        else
        {
            for (int j = 0; j < line.Length; j++)
            {
                allpattern[linenum, start++] = line[j].ToString();
            }
        }
    }
    #endregion
    #region CaseB
    public static void GetCaseB()
    {
        string m_filepath = @"e:\data2.in";
        int n = 0;//数组个数


        using (StreamReader sr = new StreamReader(m_filepath))
        {

            string firstline = sr.ReadLine();
            n = int.Parse(firstline.Trim());
            for (int i = 0; i < n; i++)
            {
                string line = sr.ReadLine();
                int x = int.Parse(line.Split(' ')[0]);
                int y = int.Parse(line.Split(' ')[1]);
                int[,] array = new int[x, y];
                for (int mm = 0; mm < x; mm++)
                {
                    string[] linesplit = sr.ReadLine().Split(' ');
                    for (int nn = 0; nn < y; nn++)
                    {
                        array[mm, nn] = int.Parse(linesplit[nn]);
                    }
                }
                GetCodeB(ref array, x, y, i);
            }
        }
    }

    public static void GetCodeB(ref int[,] array, int x, int y, int i)
    {
        //获取初始化方向表
        int[,] inittab = new int[x, y];
        for (int ii = 0; ii < x; ii++)
        {
            for (int jj = 0; jj < y; jj++)
            {
                inittab[ii, jj] = GetDiection(ref array, ii, jj, x, y);
            }
        }
        //for (int ii = 0; ii < x; ii++)
        //{
        //    for (int jj = 0; jj < y; jj++)
        //    {
        //        Console.Write(inittab[ii, jj] + " ");
        //    }
        //    Console.Write("\n");
        //}
        //获取终结点
        //Console.WriteLine("SecTab-----------");
        string[,] sectab = new string[x, y];

        for (int ii = 0; ii < x; ii++)
        {
            for (int jj = 0; jj < y; jj++)
            {
                sectab[ii, jj] = GetLastPosition(ref inittab, ii, jj, x, y);
            }
        }
        for (int ii = 0; ii < x; ii++)
        {
            for (int jj = 0; jj < y; jj++)
            {
                Console.Write(sectab[ii, jj] + " ");
            }
            Console.Write("\n");
        }


        string[,] endtab = new string[x, y];
        char max = 'a';
        for (int ii = 0; ii < x; ii++)
        {
            for (int jj = 0; jj < y; jj++)
            {
                endtab[ii, jj] = GetLastChar(ref endtab, ref sectab, ii, jj, x, y, ref max);
            }
        }
        for (int ii = 0; ii < x; ii++)
        {
            for (int jj = 0; jj < y; jj++)
            {
                Console.Write(endtab[ii, jj] + " ");
            }
            Console.Write("\n");
        }
        using (StreamWriter sw = new StreamWriter(@"e:\out2.txt", true))
        {
            sw.WriteLine(string.Format("Case #{0}:\n", i + 1));
            for (int mm = 0; mm < x; mm++)
            {

                for (int nn = 0; nn < y; nn++)
                {
                    sw.Write(endtab[mm, nn] + " ");
                }
                sw.Write(sw.NewLine);
            }

        }


    }


    /// <summary>
    /// 获取各个点的流向
    /// </summary>
    /// <param name="array"></param>
    /// <param name="ii"></param>
    /// <param name="jj"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private static int GetDiection(ref int[,] array, int ii, int jj, int x, int y)
    {
        int[] compareint = new int[5];
        compareint[0] = array[ii, jj];
        if (ii - 1 >= 0)
        {
            compareint[1] = array[ii - 1, jj];
        }
        else
        {
            compareint[1] = int.MaxValue;
        }
        if (jj - 1 >= 0)
        {
            compareint[2] = array[ii, jj - 1];
        }
        else
        {
            compareint[2] = int.MaxValue;
        }
        if (jj + 1 < y)
        {
            compareint[3] = array[ii, jj + 1];
        }
        else
        {
            compareint[3] = int.MaxValue;
        }
        if (ii + 1 < x)
        {
            compareint[4] = array[ii + 1, jj];
        }
        else
        {
            compareint[4] = int.MaxValue;
        }
        int returnval = int.MaxValue;
        int returnvalindex = 0;
        for (int i = 0; i < 5; i++)
        {
            if (compareint[i] < returnval)
            {
                returnval = compareint[i];
                returnvalindex = i;
            }
        }
        return returnvalindex;

    }
    /// <summary>
    /// 获得各个点流向的重点
    /// </summary>
    /// <param name="initab"></param>
    /// <param name="ii"></param>
    /// <param name="jj"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private static string GetLastPosition(ref int[,] initab, int ii, int jj, int x, int y)
    {
        if (initab[ii, jj] == 0)
        {
            return ii.ToString() + "," + jj.ToString();
        }
        else
        {
            int endx = ii;
            int endy = jj;
            while (initab[endx, endy] != 0)
            {
                switch (initab[endx, endy])
                {
                    case 1: endx--; break;
                    case 2: endy--; break;
                    case 3: endy++; break;
                    case 4: endx++; break;
                }
            }
            return endx.ToString() + "," + endy;
        }
    }
    /// <summary>
    /// 根据流向终点获得字符表示
    /// </summary>
    /// <param name="endtab"></param>
    /// <param name="sectab"></param>
    /// <param name="ii"></param>
    /// <param name="jj"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private static string GetLastChar(ref string[,] endtab, ref string[,] sectab, int ii, int jj, int x, int y, ref char max)
    {
        if (ii == 0 && jj == 0)
        {
            return "a";
        }
        int iii = 0;
        int jjj = 0;
        for (iii = 0; iii <= ii; iii++)
        {
            int nowj = (ii == 0 || ii == iii) ? jj : y - 1;
            for (jjj = 0; jjj <= nowj; jjj++)
            {


                if ((sectab[ii, jj] == sectab[iii, jjj]) && ((ii != iii) || (jj != jjj)))
                {
                    return endtab[iii, jjj];
                }
            }
        }
        max = Convert.ToChar(Convert.ToInt32(max) + 1);
        return max.ToString();
    }

    #endregion
    #region CaseC
    public static void GetCaseC()
    {
        string m_filepath = @"d:\data.txt";
        
        int n = 0;//模式个数

        using (StreamReader sr = new StreamReader(m_filepath))
        {

            string firstline = sr.ReadLine();
            string test = "welcotdjam";//剔除后面与前面有重复的字母
            string longtest = "welcome to code jam";
            int size = 512;
            int[,] cache = new int[size, longtest.Length];
            char[] chararry = test.ToCharArray();
            n = int.Parse(firstline.Trim());
            double[] ln = new double[n];//记录各行的匹配个数
            Console.Write(string.Format("{0}个匹配模式\n",   n));
            for (int i = 0; i < n; i++)
            {
                //Console.Write(i.ToString() + "\n");
                string str = sr.ReadLine();
                //检验是否含有所有字母
                bool miss = false;
                for(int j=0;j<chararry.Length;j++)
                {

                    int l = str.IndexOf(chararry[j]); 
                    if (l== -1)
                    {
                        miss = true; break;
                    }
                }
                if (miss) { ln[i] = 0; continue; }//还可判断index的排序来确定要不要继续判断
                 
                //获得最简字符串
                int sindex = str.IndexOf(chararry[0]);
                int eindex = str.LastIndexOf(chararry[chararry.Length - 1]);
                str = str.Substring(sindex, eindex - sindex+1);
                if (str.Length<19) { ln[i] = 0; continue; }//还可判断index的排序来确定要不要继续判断
                //List<List<int>> indexarry = new List<List<int>>(longtest.Length);
                //GetIndexArray(str, ref indexarry,longtest);
                for (int ii = 0; ii < size; ii++)
                {
                    for (int jj = 0; jj < longtest.Length; jj++)
                        cache[ii,jj] = -1;
                }

                int result = run_case(str,longtest,ref cache,longtest,str);

                Console.Write("第{0}行，{1}有{2}个匹配\n",i,str,result);
            }
             
        }
    }

    private static int run_case(string str,string test,ref int[,] cache,string oldlongtest,string oldstr)
    {
        return get_sub_string_num(str, test,ref cache,oldlongtest,oldstr);
    }

    private static int get_sub_string_num(string str, string test, ref int[,] cache, string oldlongtest, string oldstr)
    {
        if (test == string.Empty)
        {
            return 1;
        }
        if (str == string.Empty)
        {
            return 0;
        }
        if (cache[oldstr.Length - str.Length, oldlongtest.Length - test.Length] != -1)
            return cache[oldstr.Length - str.Length, oldlongtest.Length - test.Length];
        int result = 0;
        int findex=-1;
        while ((findex = str.IndexOf(test.ToCharArray()[0]))!= -1)
        {
             str=str.Substring(findex+1);
            result += get_sub_string_num(str, test.Substring(1),ref cache,oldlongtest,oldstr);

            if (result > 9999)
                result -= 10000;
        }

        return (cache[oldstr.Length - str.Length, oldlongtest.Length - test.Length] = result);

    }

 
    #endregion
    #region Helper methods

    public static void Main()
    {

        //RunSnippet();
        GetCaseD();
        RL();
    }
    #region caseD
    private static void GetCaseD()
    {
        string m_filepath = @"d:\data4.txt";

        int n = 0;//模式个数
         List<int> ll=new List<int>();
        using (StreamReader sr = new StreamReader(m_filepath))
        {

            string firstline = sr.ReadLine();
            n = int.Parse(firstline);
          
            for (int i = 0; i < n; i++)
            {
                
                 
                int searchcasenum = int.Parse(sr.ReadLine());
                string[] searchcase = new string[searchcasenum];
                for (int ii = 0; ii < searchcasenum; ii++)
                {
                    searchcase[ii] = sr.ReadLine();
                }
                int testcasenum = int.Parse(sr.ReadLine());
                string[] testcase = new string[testcasenum];
                for (int jj = 0; jj < testcasenum; jj++)
                {
                    testcase[jj] = sr.ReadLine();
                }
                int result=GetDResult(searchcasenum,testcasenum,testcase);
                ll.Add(result);
                Console.Write("第{0}行切换{1}\n", i,result);
            }

        }
        using (StreamWriter sw = new StreamWriter(@"d:\out2.txt", true))
        {
            for (int mm = 0; mm < n; mm++)
            {
                sw.WriteLine(string.Format("Case #{0}: {1}\n", mm + 1,ll[mm]));
            }

        }
    }

    private static int GetDResult(int searchcasenum, int testcasenum, string[] testcase)
    {
       
        int returnval=0;
        int result = 0;
        List<string> ls = new List<string>();
        while (result < testcasenum)
        {
            if (!ls.Contains(testcase[result]))
            {
                ls.Add(testcase[result]);
                if (ls.Count == searchcasenum )
                {
                    returnval++;
                    ls.Clear();
                    result--;
                }
            }
            result++;
        }
        return returnval;
    }
    #endregion
    private static void WL(object text, params object[] args)
    {
        Console.WriteLine(text.ToString(), args);
    }

    private static void RL()
    {
        Console.ReadLine();
    }

    private static void Break()
    {
        System.Diagnostics.Debugger.Break();
    }

    #endregion
}