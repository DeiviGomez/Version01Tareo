using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace xAPI.Library.General
{
    public class clsBanorteGenerator
    {
        /*variables step 1 */
        /*variable generica*/
        static private int _var1 = 1;

        /*variables step 2 */
        static private int _days2 = 2;
        static private int _year2013 = 2013;
        static private int _x372 = 372;
        //static int _month1 = 1;
        static private int _x31 = 31;
        //static int _day1 = 1;

        /*variables step 3 */
        static private int[] _arr731 = new int[] { 7, 3, 1 };
        static private int _var10 = 10;

        /*variables step 4 */
        static private int[] _arrPond = new int[] { 0, 13, 17, 19, 23, 11, 13, 17, 19, 23, 11, 13, 17, 19, 23, 11, 13, 17 };
        static private int _var330 = 330;
        static private int _var97 = 97;

        public static Int64 GetKeyRFID(Int64 orderid, DateTime orderdate, decimal orderamount, int minYear)
        {
            if (orderid <= 0 || orderdate <= DateTime.MinValue || orderdate.Year < minYear || orderamount <= 0) { return 0; }

            try
            {
                int[] RFID = new int[20];
                Int64[] arrOrderId;
                Int64[] arrOrderDate;
                Int64[] arrCheckers;
                Int64[] arrImportCond;

                arrOrderId = digitArr(orderid);
                arrOrderDate = GetDigitsLimitDate(orderdate);
                arrImportCond = GetDigitsImport(orderamount);
                AddArrayToRFID(arrOrderId, ref RFID, 12, 0);
                AddArrayToRFID(arrOrderDate, ref RFID, 16, 13);
                AddArrayToRFID(arrImportCond, ref RFID, 17, 17);
                arrCheckers = GetDigitsCheckers(RFID);
                AddArrayToRFID(arrCheckers, ref RFID, 19, 18);

                string key = "";
                for (int i = 0; i < RFID.Length; i++)
                {
                    if (i == RFID.Length - 1 && RFID[i] == 0) { break; }
                    key += RFID[i].ToString();
                }
                return Convert.ToInt64(key);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private static void AddArrayToRFID(Int64[] arrAdd, ref int[] RFID, int lmax, int lmin)
        {
            if (arrAdd.Length <= 0) { return; }

            int band = arrAdd.Length - 1;

            if (arrAdd.Length >= 2)
            {
                for (int i = lmax; i >= 0; i--)
                {
                    if (band >= 0 && i >= lmin)
                    {
                        RFID[i] = (int)arrAdd[band];
                        band--;
                    }
                    else { break; }
                }
            }
            else
            {
                RFID[lmin] = (int)arrAdd[0];
            }
        }


        private static Int64[] digitArr(Int64 n)
        {
            if (n == 0) return new Int64[1] { 0 };

            var digits = new List<Int64>();

            for (; n != 0; n /= 10)
                digits.Add(n % 10);

            var arr = digits.ToArray();
            Array.Reverse(arr);
            return arr;
        }

        private static Int64[] GetDigitsLimitDate(DateTime orderdate)
        {
            orderdate = Convert.ToDateTime(orderdate, CultureInfo.InvariantCulture);
            if (orderdate <= DateTime.MinValue) { return new Int64[1] { 0 }; };

            int _day = 0;
            int _month = 0;
            int _year = 0;
            int datecondensed = 0;

            orderdate = orderdate.AddDays(_days2);
            _day = orderdate.Day;
            _month = orderdate.Month;
            _year = orderdate.Year;

            datecondensed = ((_year - _year2013) * _x372) + ((_month - _var1) * _x31) + ((_day - _var1));

            return digitArr(datecondensed);
        }

        private static Int64[] GetDigitsImport(decimal amount)
        {
            if (amount <= 0) { return new Int64[1] { 0 }; };

            string str_amount = amount.ToStringDecimal();
            str_amount = str_amount.Replace(".", "");
            str_amount = str_amount.Replace(",", "");
            Int64[] arrOrderAmount = digitArr(Convert.ToInt64(str_amount));
            int n = arrOrderAmount.Length;
            int[] arrWeight = new int[n];
            int[] arrResult = new int[n];
            int ResultTotal = 0;
            int importValue = 0;
            arrWeight = BuildArrPonders(_arr731, arrOrderAmount.Length);

            for (int i = 0; i < n; i++)
            {
                arrResult[i] = (int)arrOrderAmount[i] * arrWeight[i];
                ResultTotal += arrResult[i];
            }

            importValue = (int)ResultTotal % _var10;

            return digitArr(Convert.ToInt64(importValue));
        }

        private static Int64[] GetDigitsCheckers(int[] RFID)
        {
            if (RFID.Length <= 0) { return new Int64[1] { 0 }; }

            int n = RFID.Length;
            int[] arrPonders = new int[n];
            int[] arrResult = new int[n];
            int ResultTotal = 0;
            int checkerValue = 0;

            arrPonders = BuildArrPonders(_arrPond, n - 2);

            for (int i = 0; i < n - 2; i++)
            {
                arrResult[i] = RFID[i] * arrPonders[i];
                ResultTotal += arrResult[i];
            }

            checkerValue = ((ResultTotal + _var330) % _var97) + _var1;
            return digitArr(Convert.ToInt64(checkerValue));
        }

        private static int[] BuildArrPonders(int[] arrVar, int n)
        {
            if (arrVar.Length <= 0 || n <= 0) { return new int[1] { 0 }; }

            int[] arrPonders = new int[n];
            int band = 0;
            for (int i = n - 1; i >= 0; i--)
            {
                if (band == arrVar.Length - 1)
                {
                    arrPonders[i] = arrVar[band];
                    band = 0;
                }
                else
                {
                    arrPonders[i] = arrVar[band];
                    band++;
                }
            }

            return arrPonders;
        }

    }
}
