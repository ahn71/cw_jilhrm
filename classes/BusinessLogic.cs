using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SigmaERP.classes
{
    public class BusinessLogic
    {
        static string  sql = "";
        public static DataTable get_MonthlyLoginLogOutTime(string CompanyId,  string DepartmentList, string Month, string Year, int index, string EmpCardNo, string EmpTypeID,string onlyDpt)
        {
            try
            {

                DataTable dt = new DataTable();
                if (index == 0) // 0 Means All
                    sql = "select Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpId,EmpName,sum(case DATEPART (day,AttDate) when 1 then InHour else 0 end) as '1_InH',sum(case DATEPART (day,AttDate) when 1 then InMin else 0 end) as '1_InM',sum(case DATEPART (day,AttDate) when 1 then OutHour else 0 end) as '1_OutH',sum(case DATEPART (day,AttDate) when 1 then OutMin else 0 end) as '1_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 2 then InHour else 0 end) as '2_InH',sum(case DATEPART (day,AttDate) when 2 then InMin else 0 end) as '2_InM',sum(case DATEPART (day,AttDate) when 2 then OutHour else 0 end) as '2_OutH',sum(case DATEPART (day,AttDate) when 2 then OutMin else 0 end) as '2_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 3 then InHour else 0 end) as '3_InH',sum(case DATEPART (day,AttDate) when 3 then InMin else 0 end) as '3_InM',sum(case DATEPART (day,AttDate) when 3 then OutHour else 0 end) as '3_OutH',sum(case DATEPART (day,AttDate) when 3 then OutMin else 0 end) as '3_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 4 then InHour else 0 end) as '4_InH',sum(case DATEPART (day,AttDate) when 4 then InMin else 0 end) as '4_InM',sum(case DATEPART (day,AttDate) when 4 then OutHour else 0 end) as '4_OutH',sum(case DATEPART (day,AttDate) when 4 then OutMin else 0 end) as '4_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 5 then InHour else 0 end) as '5_InH',sum(case DATEPART (day,AttDate) when 5 then InMin else 0 end) as '5_InM',sum(case DATEPART (day,AttDate) when 5 then OutHour else 0 end) as '5_OutH',sum(case DATEPART (day,AttDate) when 5 then OutMin else 0 end) as '5_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 6 then InHour else 0 end) as '6_InH',sum(case DATEPART (day,AttDate) when 6 then InMin else 0 end) as '6_InM',sum(case DATEPART (day,AttDate) when 6 then OutHour else 0 end) as '6_OutH',sum(case DATEPART (day,AttDate) when 6 then OutMin else 0 end) as '6_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 7 then InHour else 0 end) as '7_InH',sum(case DATEPART (day,AttDate) when 7 then InMin else 0 end) as '7_InM',sum(case DATEPART (day,AttDate) when 7 then OutHour else 0 end) as '7_OutH',sum(case DATEPART (day,AttDate) when 7 then OutMin else 0 end) as '7_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 8 then InHour else 0 end) as '8_InH',sum(case DATEPART (day,AttDate) when 8 then InMin else 0 end) as '8_InM',sum(case DATEPART (day,AttDate) when 8 then OutHour else 0 end) as '8_OutH',sum(case DATEPART (day,AttDate) when 8 then OutMin else 0 end) as '8_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 9 then InHour else 0 end) as '9_InH',sum(case DATEPART (day,AttDate) when 9 then InMin else 0 end) as '9_InM',sum(case DATEPART (day,AttDate) when 9 then OutHour else 0 end) as '9_OutH',sum(case DATEPART (day,AttDate) when 9 then OutMin else 0 end) as '9_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 10 then InHour else 0 end) as '10_InH',sum(case DATEPART (day,AttDate) when 10 then InMin else 0 end) as '10_InM',sum(case DATEPART (day,AttDate) when 10 then OutHour else 0 end) as '10_OutH',sum(case DATEPART (day,AttDate) when 10 then OutMin else 0 end) as '10_OutM'," +

                    "sum(case DATEPART (day,AttDate) when 11 then InHour else 0 end) as '11_InH',sum(case DATEPART (day,AttDate) when 11 then InMin else 0 end) as '11_InM',sum(case DATEPART (day,AttDate) when 11 then OutHour else 0 end) as '11_OutH',sum(case DATEPART (day,AttDate) when 11 then OutMin else 0 end) as '11_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 12 then InHour else 0 end) as '12_InH',sum(case DATEPART (day,AttDate) when 12 then InMin else 0 end) as '12_InM',sum(case DATEPART (day,AttDate) when 12 then OutHour else 0 end) as '12_OutH',sum(case DATEPART (day,AttDate) when 12 then OutMin else 0 end) as '12_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 13 then InHour else 0 end) as '13_InH',sum(case DATEPART (day,AttDate) when 13 then InMin else 0 end) as '13_InM',sum(case DATEPART (day,AttDate) when 13 then OutHour else 0 end) as '13_OutH',sum(case DATEPART (day,AttDate) when 13 then OutMin else 0 end) as '13_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 14 then InHour else 0 end) as '14_InH',sum(case DATEPART (day,AttDate) when 14 then InMin else 0 end) as '14_InM',sum(case DATEPART (day,AttDate) when 14 then OutHour else 0 end) as '14_OutH',sum(case DATEPART (day,AttDate) when 14 then OutMin else 0 end) as '14_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 15 then InHour else 0 end) as '15_InH',sum(case DATEPART (day,AttDate) when 15 then InMin else 0 end) as '15_InM',sum(case DATEPART (day,AttDate) when 15 then OutHour else 0 end) as '15_OutH',sum(case DATEPART (day,AttDate) when 15 then OutMin else 0 end) as '15_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 16 then InHour else 0 end) as '16_InH',sum(case DATEPART (day,AttDate) when 16 then InMin else 0 end) as '16_InM',sum(case DATEPART (day,AttDate) when 16 then OutHour else 0 end) as '16_OutH',sum(case DATEPART (day,AttDate) when 16 then OutMin else 0 end) as '16_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 17 then InHour else 0 end) as '17_InH',sum(case DATEPART (day,AttDate) when 17 then InMin else 0 end) as '17_InM',sum(case DATEPART (day,AttDate) when 17 then OutHour else 0 end) as '17_OutH',sum(case DATEPART (day,AttDate) when 17 then OutMin else 0 end) as '17_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 18 then InHour else 0 end) as '18_InH',sum(case DATEPART (day,AttDate) when 18 then InMin else 0 end) as '18_InM',sum(case DATEPART (day,AttDate) when 18 then OutHour else 0 end) as '18_OutH',sum(case DATEPART (day,AttDate) when 18 then OutMin else 0 end) as '18_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 19 then InHour else 0 end) as '19_InH',sum(case DATEPART (day,AttDate) when 19 then InMin else 0 end) as '19_InM',sum(case DATEPART (day,AttDate) when 19 then OutHour else 0 end) as '19_OutH',sum(case DATEPART (day,AttDate) when 19 then OutMin else 0 end) as '19_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 20 then InHour else 0 end) as '20_InH',sum(case DATEPART (day,AttDate) when 20 then InMin else 0 end) as '20_InM',sum(case DATEPART (day,AttDate) when 20 then OutHour else 0 end) as '20_OutH',sum(case DATEPART (day,AttDate) when 20 then OutMin else 0 end) as '20_OutM'," +

                    "sum(case DATEPART (day,AttDate) when 21 then InHour else 0 end) as '21_InH',sum(case DATEPART (day,AttDate) when 21 then InMin else 0 end) as '21_InM',sum(case DATEPART (day,AttDate) when 21 then OutHour else 0 end) as '21_OutH',sum(case DATEPART (day,AttDate) when 21 then OutMin else 0 end) as '21_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 22 then InHour else 0 end) as '22_InH',sum(case DATEPART (day,AttDate) when 22 then InMin else 0 end) as '22_InM',sum(case DATEPART (day,AttDate) when 22 then OutHour else 0 end) as '22_OutH',sum(case DATEPART (day,AttDate) when 22 then OutMin else 0 end) as '22_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 23 then InHour else 0 end) as '23_InH',sum(case DATEPART (day,AttDate) when 23 then InMin else 0 end) as '23_InM',sum(case DATEPART (day,AttDate) when 23 then OutHour else 0 end) as '23_OutH',sum(case DATEPART (day,AttDate) when 23 then OutMin else 0 end) as '23_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 24 then InHour else 0 end) as '24_InH',sum(case DATEPART (day,AttDate) when 24 then InMin else 0 end) as '24_InM',sum(case DATEPART (day,AttDate) when 24 then OutHour else 0 end) as '24_OutH',sum(case DATEPART (day,AttDate) when 24 then OutMin else 0 end) as '24_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 25 then InHour else 0 end) as '25_InH',sum(case DATEPART (day,AttDate) when 25 then InMin else 0 end) as '25_InM',sum(case DATEPART (day,AttDate) when 25 then OutHour else 0 end) as '25_OutH',sum(case DATEPART (day,AttDate) when 25 then OutMin else 0 end) as '25_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 26 then InHour else 0 end) as '26_InH',sum(case DATEPART (day,AttDate) when 26 then InMin else 0 end) as '26_InM',sum(case DATEPART (day,AttDate) when 26 then OutHour else 0 end) as '26_OutH',sum(case DATEPART (day,AttDate) when 26 then OutMin else 0 end) as '26_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 27 then InHour else 0 end) as '27_InH',sum(case DATEPART (day,AttDate) when 27 then InMin else 0 end) as '27_InM',sum(case DATEPART (day,AttDate) when 27 then OutHour else 0 end) as '27_OutH',sum(case DATEPART (day,AttDate) when 27 then OutMin else 0 end) as '27_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 28 then InHour else 0 end) as '28_InH',sum(case DATEPART (day,AttDate) when 28 then InMin else 0 end) as '28_InM',sum(case DATEPART (day,AttDate) when 28 then OutHour else 0 end) as '28_OutH',sum(case DATEPART (day,AttDate) when 28 then OutMin else 0 end) as '28_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 29 then InHour else 0 end) as '29_InH',sum(case DATEPART (day,AttDate) when 29 then InMin else 0 end) as '29_InM',sum(case DATEPART (day,AttDate) when 29 then OutHour else 0 end) as '29_OutH',sum(case DATEPART (day,AttDate) when 29 then OutMin else 0 end) as '29_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 30 then InHour else 0 end) as '30_InH',sum(case DATEPART (day,AttDate) when 30 then InMin else 0 end) as '30_InM',sum(case DATEPART (day,AttDate) when 30 then OutHour else 0 end) as '30_OutH',sum(case DATEPART (day,AttDate) when 30 then OutMin else 0 end) as '30_OutM'," +
                    "sum(case DATEPART (day,AttDate) when 31 then InHour else 0 end) as '31_InH',sum(case DATEPART (day,AttDate) when 31 then InMin else 0 end) as '31_InM',sum(case DATEPART (day,AttDate) when 31 then OutHour else 0 end) as '31_OutH',sum(case DATEPART (day,AttDate) when 31 then OutMin else 0 end) as '31_OutM'" +
                    ",DptId,DptName,SftId,SftName,CompanyName,Address " +
                    "from v_tblAttendanceRecord " +
                    "Where CompanyId " + CompanyId + "   AND DptId " + DepartmentList + " " + EmpTypeID + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "'" +
                    "group by EmpCardNo, EmpId,EmpName,DptId,DptName,SftId,SftName,CompanyName,Address,convert(int,DptCode), convert(int,SftId),CustomOrdering " +
                            " order by convert(int,DptCode), convert(int,SftId),CustomOrdering";
                else
                    sql = "select Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo, EmpId,EmpName,sum(case DATEPART (day,AttDate) when 1 then InHour else 0 end) as '1_InH',sum(case DATEPART (day,AttDate) when 1 then InMin else 0 end) as '1_InM',sum(case DATEPART (day,AttDate) when 1 then OutHour else 0 end) as '1_OutH',sum(case DATEPART (day,AttDate) when 1 then OutMin else 0 end) as '1_OutM'," +
                "sum(case DATEPART (day,AttDate) when 2 then InHour else 0 end) as '2_InH',sum(case DATEPART (day,AttDate) when 2 then InMin else 0 end) as '2_InM',sum(case DATEPART (day,AttDate) when 2 then OutHour else 0 end) as '2_OutH',sum(case DATEPART (day,AttDate) when 2 then OutMin else 0 end) as '2_OutM'," +
                "sum(case DATEPART (day,AttDate) when 3 then InHour else 0 end) as '3_InH',sum(case DATEPART (day,AttDate) when 3 then InMin else 0 end) as '3_InM',sum(case DATEPART (day,AttDate) when 3 then OutHour else 0 end) as '3_OutH',sum(case DATEPART (day,AttDate) when 3 then OutMin else 0 end) as '3_OutM'," +
                "sum(case DATEPART (day,AttDate) when 4 then InHour else 0 end) as '4_InH',sum(case DATEPART (day,AttDate) when 4 then InMin else 0 end) as '4_InM',sum(case DATEPART (day,AttDate) when 4 then OutHour else 0 end) as '4_OutH',sum(case DATEPART (day,AttDate) when 4 then OutMin else 0 end) as '4_OutM'," +
                "sum(case DATEPART (day,AttDate) when 5 then InHour else 0 end) as '5_InH',sum(case DATEPART (day,AttDate) when 5 then InMin else 0 end) as '5_InM',sum(case DATEPART (day,AttDate) when 5 then OutHour else 0 end) as '5_OutH',sum(case DATEPART (day,AttDate) when 5 then OutMin else 0 end) as '5_OutM'," +
                "sum(case DATEPART (day,AttDate) when 6 then InHour else 0 end) as '6_InH',sum(case DATEPART (day,AttDate) when 6 then InMin else 0 end) as '6_InM',sum(case DATEPART (day,AttDate) when 6 then OutHour else 0 end) as '6_OutH',sum(case DATEPART (day,AttDate) when 6 then OutMin else 0 end) as '6_OutM'," +
                "sum(case DATEPART (day,AttDate) when 7 then InHour else 0 end) as '7_InH',sum(case DATEPART (day,AttDate) when 7 then InMin else 0 end) as '7_InM',sum(case DATEPART (day,AttDate) when 7 then OutHour else 0 end) as '7_OutH',sum(case DATEPART (day,AttDate) when 7 then OutMin else 0 end) as '7_OutM'," +
                "sum(case DATEPART (day,AttDate) when 8 then InHour else 0 end) as '8_InH',sum(case DATEPART (day,AttDate) when 8 then InMin else 0 end) as '8_InM',sum(case DATEPART (day,AttDate) when 8 then OutHour else 0 end) as '8_OutH',sum(case DATEPART (day,AttDate) when 8 then OutMin else 0 end) as '8_OutM'," +
                "sum(case DATEPART (day,AttDate) when 9 then InHour else 0 end) as '9_InH',sum(case DATEPART (day,AttDate) when 9 then InMin else 0 end) as '9_InM',sum(case DATEPART (day,AttDate) when 9 then OutHour else 0 end) as '9_OutH',sum(case DATEPART (day,AttDate) when 9 then OutMin else 0 end) as '9_OutM'," +
                "sum(case DATEPART (day,AttDate) when 10 then InHour else 0 end) as '10_InH',sum(case DATEPART (day,AttDate) when 10 then InMin else 0 end) as '10_InM',sum(case DATEPART (day,AttDate) when 10 then OutHour else 0 end) as '10_OutH',sum(case DATEPART (day,AttDate) when 10 then OutMin else 0 end) as '10_OutM'," +

                "sum(case DATEPART (day,AttDate) when 11 then InHour else 0 end) as '11_InH',sum(case DATEPART (day,AttDate) when 11 then InMin else 0 end) as '11_InM',sum(case DATEPART (day,AttDate) when 11 then OutHour else 0 end) as '11_OutH',sum(case DATEPART (day,AttDate) when 11 then OutMin else 0 end) as '11_OutM'," +
                "sum(case DATEPART (day,AttDate) when 12 then InHour else 0 end) as '12_InH',sum(case DATEPART (day,AttDate) when 12 then InMin else 0 end) as '12_InM',sum(case DATEPART (day,AttDate) when 12 then OutHour else 0 end) as '12_OutH',sum(case DATEPART (day,AttDate) when 12 then OutMin else 0 end) as '12_OutM'," +
                "sum(case DATEPART (day,AttDate) when 13 then InHour else 0 end) as '13_InH',sum(case DATEPART (day,AttDate) when 13 then InMin else 0 end) as '13_InM',sum(case DATEPART (day,AttDate) when 13 then OutHour else 0 end) as '13_OutH',sum(case DATEPART (day,AttDate) when 13 then OutMin else 0 end) as '13_OutM'," +
                "sum(case DATEPART (day,AttDate) when 14 then InHour else 0 end) as '14_InH',sum(case DATEPART (day,AttDate) when 14 then InMin else 0 end) as '14_InM',sum(case DATEPART (day,AttDate) when 14 then OutHour else 0 end) as '14_OutH',sum(case DATEPART (day,AttDate) when 14 then OutMin else 0 end) as '14_OutM'," +
                "sum(case DATEPART (day,AttDate) when 15 then InHour else 0 end) as '15_InH',sum(case DATEPART (day,AttDate) when 15 then InMin else 0 end) as '15_InM',sum(case DATEPART (day,AttDate) when 15 then OutHour else 0 end) as '15_OutH',sum(case DATEPART (day,AttDate) when 15 then OutMin else 0 end) as '15_OutM'," +
                "sum(case DATEPART (day,AttDate) when 16 then InHour else 0 end) as '16_InH',sum(case DATEPART (day,AttDate) when 16 then InMin else 0 end) as '16_InM',sum(case DATEPART (day,AttDate) when 16 then OutHour else 0 end) as '16_OutH',sum(case DATEPART (day,AttDate) when 16 then OutMin else 0 end) as '16_OutM'," +
                "sum(case DATEPART (day,AttDate) when 17 then InHour else 0 end) as '17_InH',sum(case DATEPART (day,AttDate) when 17 then InMin else 0 end) as '17_InM',sum(case DATEPART (day,AttDate) when 17 then OutHour else 0 end) as '17_OutH',sum(case DATEPART (day,AttDate) when 17 then OutMin else 0 end) as '17_OutM'," +
                "sum(case DATEPART (day,AttDate) when 18 then InHour else 0 end) as '18_InH',sum(case DATEPART (day,AttDate) when 18 then InMin else 0 end) as '18_InM',sum(case DATEPART (day,AttDate) when 18 then OutHour else 0 end) as '18_OutH',sum(case DATEPART (day,AttDate) when 18 then OutMin else 0 end) as '18_OutM'," +
                "sum(case DATEPART (day,AttDate) when 19 then InHour else 0 end) as '19_InH',sum(case DATEPART (day,AttDate) when 19 then InMin else 0 end) as '19_InM',sum(case DATEPART (day,AttDate) when 19 then OutHour else 0 end) as '19_OutH',sum(case DATEPART (day,AttDate) when 19 then OutMin else 0 end) as '19_OutM'," +
                "sum(case DATEPART (day,AttDate) when 20 then InHour else 0 end) as '20_InH',sum(case DATEPART (day,AttDate) when 20 then InMin else 0 end) as '20_InM',sum(case DATEPART (day,AttDate) when 20 then OutHour else 0 end) as '20_OutH',sum(case DATEPART (day,AttDate) when 20 then OutMin else 0 end) as '20_OutM'," +

                "sum(case DATEPART (day,AttDate) when 21 then InHour else 0 end) as '21_InH',sum(case DATEPART (day,AttDate) when 21 then InMin else 0 end) as '21_InM',sum(case DATEPART (day,AttDate) when 21 then OutHour else 0 end) as '21_OutH',sum(case DATEPART (day,AttDate) when 21 then OutMin else 0 end) as '21_OutM'," +
                "sum(case DATEPART (day,AttDate) when 22 then InHour else 0 end) as '22_InH',sum(case DATEPART (day,AttDate) when 22 then InMin else 0 end) as '22_InM',sum(case DATEPART (day,AttDate) when 22 then OutHour else 0 end) as '22_OutH',sum(case DATEPART (day,AttDate) when 22 then OutMin else 0 end) as '22_OutM'," +
                "sum(case DATEPART (day,AttDate) when 23 then InHour else 0 end) as '23_InH',sum(case DATEPART (day,AttDate) when 23 then InMin else 0 end) as '23_InM',sum(case DATEPART (day,AttDate) when 23 then OutHour else 0 end) as '23_OutH',sum(case DATEPART (day,AttDate) when 23 then OutMin else 0 end) as '23_OutM'," +
                "sum(case DATEPART (day,AttDate) when 24 then InHour else 0 end) as '24_InH',sum(case DATEPART (day,AttDate) when 24 then InMin else 0 end) as '24_InM',sum(case DATEPART (day,AttDate) when 24 then OutHour else 0 end) as '24_OutH',sum(case DATEPART (day,AttDate) when 24 then OutMin else 0 end) as '24_OutM'," +
                "sum(case DATEPART (day,AttDate) when 25 then InHour else 0 end) as '25_InH',sum(case DATEPART (day,AttDate) when 25 then InMin else 0 end) as '25_InM',sum(case DATEPART (day,AttDate) when 25 then OutHour else 0 end) as '25_OutH',sum(case DATEPART (day,AttDate) when 25 then OutMin else 0 end) as '25_OutM'," +
                "sum(case DATEPART (day,AttDate) when 26 then InHour else 0 end) as '26_InH',sum(case DATEPART (day,AttDate) when 26 then InMin else 0 end) as '26_InM',sum(case DATEPART (day,AttDate) when 26 then OutHour else 0 end) as '26_OutH',sum(case DATEPART (day,AttDate) when 26 then OutMin else 0 end) as '26_OutM'," +
                "sum(case DATEPART (day,AttDate) when 27 then InHour else 0 end) as '27_InH',sum(case DATEPART (day,AttDate) when 27 then InMin else 0 end) as '27_InM',sum(case DATEPART (day,AttDate) when 27 then OutHour else 0 end) as '27_OutH',sum(case DATEPART (day,AttDate) when 27 then OutMin else 0 end) as '27_OutM'," +
                "sum(case DATEPART (day,AttDate) when 28 then InHour else 0 end) as '28_InH',sum(case DATEPART (day,AttDate) when 28 then InMin else 0 end) as '28_InM',sum(case DATEPART (day,AttDate) when 28 then OutHour else 0 end) as '28_OutH',sum(case DATEPART (day,AttDate) when 28 then OutMin else 0 end) as '28_OutM'," +
                "sum(case DATEPART (day,AttDate) when 29 then InHour else 0 end) as '29_InH',sum(case DATEPART (day,AttDate) when 29 then InMin else 0 end) as '29_InM',sum(case DATEPART (day,AttDate) when 29 then OutHour else 0 end) as '29_OutH',sum(case DATEPART (day,AttDate) when 29 then OutMin else 0 end) as '29_OutM'," +
                "sum(case DATEPART (day,AttDate) when 30 then InHour else 0 end) as '30_InH',sum(case DATEPART (day,AttDate) when 30 then InMin else 0 end) as '30_InM',sum(case DATEPART (day,AttDate) when 30 then OutHour else 0 end) as '30_OutH',sum(case DATEPART (day,AttDate) when 30 then OutMin else 0 end) as '30_OutM'," +
                "sum(case DATEPART (day,AttDate) when 31 then InHour else 0 end) as '31_InH',sum(case DATEPART (day,AttDate) when 31 then InMin else 0 end) as '31_InM',sum(case DATEPART (day,AttDate) when 31 then OutHour else 0 end) as '31_OutH',sum(case DATEPART (day,AttDate) when 31 then OutMin else 0 end) as '31_OutM'" +
                ",DptId,DptName,SftId,SftName,CompanyName,Address " +
                "from v_tblAttendanceRecord " +
                "Where CompanyId " + CompanyId + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "' AND Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + EmpCardNo + "' "+ onlyDpt + " " +
                "group by EmpCardNo, EmpId,EmpName,DptId,DptName,SftId,SftName,CompanyName,Address ";
                    sqlDB.fillDataTable(sql, dt = new DataTable());
                return dt;
            }
            catch { return null; }
        }


        public static DataTable get_Moanthly_Attendance_Sheet(string CompanyId,  string DepartmentList, string Month, string Year, int index, string EmpCardNo, string EmpTypeID,string onlyDpt)
        {
            try
            {
                DataTable dt=new DataTable ();
                if (index == 0)
                    sql = "select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName," +
                        "sum(case DATEPART (day,AttDate) when 1 then code else 0 end) as '1'," +
                        "sum(case DATEPART (day,AttDate) when 2 then code else 0 end) as '2'," +
                        "sum(case DATEPART (day,AttDate) when 3 then code else 0 end) as '3'," +
                        "sum(case DATEPART (day,AttDate) when 4 then code else 0 end) as '4'," +
                        "sum(case DATEPART (day,AttDate) when 5 then code else 0 end) as '5'," +
                        "sum(case DATEPART (day,AttDate) when 6 then code else 0 end) as '6'," +
                        "sum(case DATEPART (day,AttDate) when 7 then code else 0 end) as '7'," +
                        "sum(case DATEPART (day,AttDate) when 8 then code else 0 end) as '8'," +
                        "sum(case DATEPART (day,AttDate) when 9 then code else 0 end) as '9'," +
                        "sum(case DATEPART (day,AttDate) when 10 then code else 0 end) as '10'," +
                        "sum(case DATEPART (day,AttDate) when 11 then code else 0 end) as '11'," +
                        "sum(case DATEPART (day,AttDate) when 12 then code else 0 end) as '12'," +
                        "sum(case DATEPART (day,AttDate) when 13 then code else 0 end) as '13'," +
                        "sum(case DATEPART (day,AttDate) when 14 then code else 0 end) as '14'," +
                        "sum(case DATEPART (day,AttDate) when 15 then code else 0 end) as '15'," +
                        "sum(case DATEPART (day,AttDate) when 16 then code else 0 end) as '1'," +
                        "sum(case DATEPART (day,AttDate) when 17 then code else 0 end) as '17'," +
                        "sum(case DATEPART (day,AttDate) when 18 then code else 0 end) as '18'," +
                        "sum(case DATEPART (day,AttDate) when 19 then code else 0 end) as '19'," +
                        "sum(case DATEPART (day,AttDate) when 20 then code else 0 end) as '20'," +
                        "sum(case DATEPART (day,AttDate) when 21 then code else 0 end) as '21'," +
                        "sum(case DATEPART (day,AttDate) when 22 then code else 0 end) as '22'," +
                        "sum(case DATEPART (day,AttDate) when 23 then code else 0 end) as '23'," +
                        "sum(case DATEPART (day,AttDate) when 24 then code else 0 end) as '24'," +
                        "sum(case DATEPART (day,AttDate) when 25 then code else 0 end) as '25'," +
                        "sum(case DATEPART (day,AttDate) when 26 then code else 0 end) as '26'," +
                        "sum(case DATEPART (day,AttDate) when 27 then code else 0 end) as '27'," +
                        "sum(case DATEPART (day,AttDate) when 28 then code else 0 end) as '28'," +
                        "sum(case DATEPART (day,AttDate) when 29 then code else 0 end) as '29'," +
                        "sum(case DATEPART (day,AttDate) when 30 then code else 0 end) as '30'," +
                        "sum(case DATEPART (day,AttDate) when 31 then code else 0 end) as '31'," +
                        " DsgName, DptId,DptName,SftId,SftName,CompanyId,CompanyName,Address " +
                        "from v_tblAttendanceRecord " +
                        "Where CompanyId " + CompanyId + "  AND DptId " + DepartmentList + " " + EmpTypeID + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "'" +
                        "group by EmpId,EmpCardNo,EmpName, DsgName,DptId,DptName,SftId,SftName,CompanyId,CompanyName,Address,convert(int,DptCode), convert(int,SftId),CustomOrdering " +
                            " order by convert(int,DptCode), convert(int,SftId),CustomOrdering";
                else
                    sql = "select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName," +
                    "sum(case DATEPART (day,AttDate) when 1 then code else 0 end) as '1'," +
                    "sum(case DATEPART (day,AttDate) when 2 then code else 0 end) as '2'," +
                    "sum(case DATEPART (day,AttDate) when 3 then code else 0 end) as '3'," +
                    "sum(case DATEPART (day,AttDate) when 4 then code else 0 end) as '4'," +
                    "sum(case DATEPART (day,AttDate) when 5 then code else 0 end) as '5'," +
                    "sum(case DATEPART (day,AttDate) when 6 then code else 0 end) as '6'," +
                    "sum(case DATEPART (day,AttDate) when 7 then code else 0 end) as '7'," +
                    "sum(case DATEPART (day,AttDate) when 8 then code else 0 end) as '8'," +
                    "sum(case DATEPART (day,AttDate) when 9 then code else 0 end) as '9'," +
                    "sum(case DATEPART (day,AttDate) when 10 then code else 0 end) as '10'," +
                    "sum(case DATEPART (day,AttDate) when 11 then code else 0 end) as '11'," +
                    "sum(case DATEPART (day,AttDate) when 12 then code else 0 end) as '12'," +
                    "sum(case DATEPART (day,AttDate) when 13 then code else 0 end) as '13'," +
                    "sum(case DATEPART (day,AttDate) when 14 then code else 0 end) as '14'," +
                    "sum(case DATEPART (day,AttDate) when 15 then code else 0 end) as '15'," +
                    "sum(case DATEPART (day,AttDate) when 16 then code else 0 end) as '1'," +
                    "sum(case DATEPART (day,AttDate) when 17 then code else 0 end) as '17'," +
                    "sum(case DATEPART (day,AttDate) when 18 then code else 0 end) as '18'," +
                    "sum(case DATEPART (day,AttDate) when 19 then code else 0 end) as '19'," +
                    "sum(case DATEPART (day,AttDate) when 20 then code else 0 end) as '20'," +
                    "sum(case DATEPART (day,AttDate) when 21 then code else 0 end) as '21'," +
                    "sum(case DATEPART (day,AttDate) when 22 then code else 0 end) as '22'," +
                    "sum(case DATEPART (day,AttDate) when 23 then code else 0 end) as '23'," +
                    "sum(case DATEPART (day,AttDate) when 24 then code else 0 end) as '24'," +
                    "sum(case DATEPART (day,AttDate) when 25 then code else 0 end) as '25'," +
                    "sum(case DATEPART (day,AttDate) when 26 then code else 0 end) as '26'," +
                    "sum(case DATEPART (day,AttDate) when 27 then code else 0 end) as '27'," +
                    "sum(case DATEPART (day,AttDate) when 28 then code else 0 end) as '28'," +
                    "sum(case DATEPART (day,AttDate) when 29 then code else 0 end) as '29'," +
                    "sum(case DATEPART (day,AttDate) when 30 then code else 0 end) as '30'," +
                    "sum(case DATEPART (day,AttDate) when 31 then code else 0 end) as '31'," +
                    "  DsgName,DptId,DptName,SftId,SftName,CompanyId,CompanyName,Address " +
                    "from v_tblAttendanceRecord " +
                    "Where CompanyId " + CompanyId + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "' AND Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + EmpCardNo + "'" +
                    " "+ onlyDpt + " group by EmpId,EmpCardNo,EmpName,DptId, DsgName,DptName,SftId,SftName,CompanyId,CompanyName,Address ";

                sqlDB.fillDataTable(sql, dt = new DataTable());
                return dt;
            }
            catch { return null; }
        }

        public static DataTable get_Moanthly_Attendance_Sheet_Summary(string CompanyId, string DepartmentList, string Month, string Year, int index, string EmpCardNo, string EmpTypeID,string onlyDpt)
        {
            try
            {
                DataTable dt=new DataTable ();
                if (index == 0)
                    sql = "SELECT        EmpId, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo, EmpName,Address,GId,GName, SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN code ELSE 0 END) AS [1], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN code ELSE 0 END) AS [2]," +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN code ELSE 0 END) AS [3], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN code ELSE 0 END) AS [4], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 5 THEN code ELSE 0 END) AS [5], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN code ELSE 0 END) AS [6], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN code ELSE 0 END) AS [7]," +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN code ELSE 0 END) AS [8], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN code ELSE 0 END) AS [9], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 10 THEN code ELSE 0 END) AS [10], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN code ELSE 0 END) AS [11], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN code ELSE 0 END) AS [12]," +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN code ELSE 0 END) AS [13], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN code ELSE 0 END) AS [14], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 15 THEN code ELSE 0 END) AS [15], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN code ELSE 0 END) AS [16], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN code ELSE 0 END) AS [17], " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN code ELSE 0 END) AS [18], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN code ELSE 0 END) AS [19], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 20 THEN code ELSE 0 END) AS [20], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN code ELSE 0 END) AS [21], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN code ELSE 0 END) AS [22], " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN code ELSE 0 END) AS [23], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN code ELSE 0 END) AS [24], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 25 THEN code ELSE 0 END) AS [25], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN code ELSE 0 END) AS [26], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN code ELSE 0 END) AS [27], " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN code ELSE 0 END) AS [28], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN code ELSE 0 END) AS [29], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 30 THEN code ELSE 0 END) AS [30], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN code ELSE 0 END) AS [31], DsgName, DptId, DptName, CompanyId, CompanyName," +
                          "(SUM(case Code when 112 then 1 else 0 end)+SUM(case Code when 108 then 1 else 0 end)+SUM(case Code when 104  then 1 else 0 end)+SUM(case Code when 119  then 1 else 0 end)+SUM(case Code when 207  then 1 else 0 end)+SUM(case Code when 223  then 1 else 0 end)+SUM(case Code when 205  then 1 else 0 end)+SUM(case Code when 217  then 1 else 0 end)) as P,SUM(case Code when 97 then 1 else 0 end) as A ,SUM(case Code when 108 then 1 else 0 end) as L,SUM(case Code when 104  then 1 else 0 end) as H,SUM(case Code when 119  then 1 else 0 end) as W," +
                          "(SUM(case Code when 207  then 1 else 0 end)+SUM(case Code when 223  then 1 else 0 end)+SUM(case Code when 205  then 1 else 0 end)+SUM(case Code when 217  then 1 else 0 end)+SUM(case Code when 210  then 1 else 0 end)+SUM(case Code when 229  then 1 else 0 end)+SUM(case Code when 228  then 1 else 0 end)) as LV,SUM(case Code when 227  then 1 else 0 end) as DptCode " +
                          "   FROM            dbo.v_tblAttendanceRecord " +
                          "   Where CompanyId " + CompanyId + " AND DptId " + DepartmentList + " " + EmpTypeID + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "'" +
                          "   GROUP BY EmpId, EmpCardNo, EmpName, DsgName, DptId, DptName,GId,GName, CompanyId, CompanyName,Address,CustomOrdering  " +
                         " order by convert(int,DptId), convert(int,GId),CustomOrdering";
                else
                    sql = "SELECT        EmpId, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo, EmpName,Address,GId,GName, SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN code ELSE 0 END) AS [1], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN code ELSE 0 END) AS [2]," +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN code ELSE 0 END) AS [3], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN code ELSE 0 END) AS [4], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 5 THEN code ELSE 0 END) AS [5], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN code ELSE 0 END) AS [6], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN code ELSE 0 END) AS [7]," +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN code ELSE 0 END) AS [8], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN code ELSE 0 END) AS [9], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 10 THEN code ELSE 0 END) AS [10], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN code ELSE 0 END) AS [11], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN code ELSE 0 END) AS [12]," +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN code ELSE 0 END) AS [13], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN code ELSE 0 END) AS [14], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 15 THEN code ELSE 0 END) AS [15], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN code ELSE 0 END) AS [16], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN code ELSE 0 END) AS [17], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN code ELSE 0 END) AS [18], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN code ELSE 0 END) AS [19], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 20 THEN code ELSE 0 END) AS [20], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN code ELSE 0 END) AS [21], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN code ELSE 0 END) AS [22], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN code ELSE 0 END) AS [23], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN code ELSE 0 END) AS [24], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 25 THEN code ELSE 0 END) AS [25], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN code ELSE 0 END) AS [26], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN code ELSE 0 END) AS [27], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN code ELSE 0 END) AS [28], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN code ELSE 0 END) AS [29], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 30 THEN code ELSE 0 END) AS [30], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN code ELSE 0 END) AS [31], DsgName, DptId, DptName,  CompanyId, CompanyName," +
                         "(SUM(case Code when 112 then 1 else 0 end)+SUM(case Code when 108 then 1 else 0 end)+SUM(case Code when 104  then 1 else 0 end)+SUM(case Code when 119  then 1 else 0 end)+SUM(case Code when 207  then 1 else 0 end)+SUM(case Code when 223  then 1 else 0 end)+SUM(case Code when 205  then 1 else 0 end)+SUM(case Code when 217  then 1 else 0 end)) as P,SUM(case Code when 97 then 1 else 0 end) as A ,SUM(case Code when 108 then 1 else 0 end) as L,SUM(case Code when 104  then 1 else 0 end) as H,SUM(case Code when 119  then 1 else 0 end) as W," +
                         "(SUM(case Code when 207  then 1 else 0 end)+SUM(case Code when 223  then 1 else 0 end)+SUM(case Code when 205  then 1 else 0 end)+SUM(case Code when 217  then 1 else 0 end)+SUM(case Code when 210  then 1 else 0 end)+SUM(case Code when 229  then 1 else 0 end)+SUM(case Code when 228  then 1 else 0 end)) as LV,SUM(case Code when 227  then 1 else 0 end) as DptCode " +
                        "   FROM            dbo.v_tblAttendanceRecord " +
                        "   Where CompanyId " + CompanyId + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "' AND Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + EmpCardNo + "'" +
                        "  "+ onlyDpt + "  GROUP BY EmpId, EmpCardNo, EmpName, DsgName, DptId, DptName,GId,GName, CompanyId, CompanyName,Address,CustomOrdering";

                sqlDB.fillDataTable(sql, dt);
                return dt;
            }
            catch {return null; }
        }

        //---------------------------------------For Bangla Report----------------------------------------------

        public static DataTable get_MonthlyLoginLogOutTimeBangla(string CompanyId, string DepartmentList, string Month, string Year, int index, string EmpCardNo, string EmpTypeID)
        {
            try
            {

                DataTable dt = new DataTable();
                if (index == 0) // 0 Means All
                    sql = "select Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpId,EmpNameBn EmpName,sum(case DATEPART (day,AttDate) when 1 then InHour else 0 end) as '1_InH',sum(case DATEPART (day,AttDate) when 1 then InMin else 0 end) as '1_InM',sum(case DATEPART (day,AttDate) when 1 then OutHour else 0 end) as '1_OutH',sum(case DATEPART (day,AttDate) when 1 then OutMin else 0 end) as '1_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 2 then InHour else 0 end) as '2_InH',sum(case DATEPART (day,AttDate) when 2 then InMin else 0 end) as '2_InM',sum(case DATEPART (day,AttDate) when 2 then OutHour else 0 end) as '2_OutH',sum(case DATEPART (day,AttDate) when 2 then OutMin else 0 end) as '2_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 3 then InHour else 0 end) as '3_InH',sum(case DATEPART (day,AttDate) when 3 then InMin else 0 end) as '3_InM',sum(case DATEPART (day,AttDate) when 3 then OutHour else 0 end) as '3_OutH',sum(case DATEPART (day,AttDate) when 3 then OutMin else 0 end) as '3_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 4 then InHour else 0 end) as '4_InH',sum(case DATEPART (day,AttDate) when 4 then InMin else 0 end) as '4_InM',sum(case DATEPART (day,AttDate) when 4 then OutHour else 0 end) as '4_OutH',sum(case DATEPART (day,AttDate) when 4 then OutMin else 0 end) as '4_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 5 then InHour else 0 end) as '5_InH',sum(case DATEPART (day,AttDate) when 5 then InMin else 0 end) as '5_InM',sum(case DATEPART (day,AttDate) when 5 then OutHour else 0 end) as '5_OutH',sum(case DATEPART (day,AttDate) when 5 then OutMin else 0 end) as '5_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 6 then InHour else 0 end) as '6_InH',sum(case DATEPART (day,AttDate) when 6 then InMin else 0 end) as '6_InM',sum(case DATEPART (day,AttDate) when 6 then OutHour else 0 end) as '6_OutH',sum(case DATEPART (day,AttDate) when 6 then OutMin else 0 end) as '6_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 7 then InHour else 0 end) as '7_InH',sum(case DATEPART (day,AttDate) when 7 then InMin else 0 end) as '7_InM',sum(case DATEPART (day,AttDate) when 7 then OutHour else 0 end) as '7_OutH',sum(case DATEPART (day,AttDate) when 7 then OutMin else 0 end) as '7_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 8 then InHour else 0 end) as '8_InH',sum(case DATEPART (day,AttDate) when 8 then InMin else 0 end) as '8_InM',sum(case DATEPART (day,AttDate) when 8 then OutHour else 0 end) as '8_OutH',sum(case DATEPART (day,AttDate) when 8 then OutMin else 0 end) as '8_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 9 then InHour else 0 end) as '9_InH',sum(case DATEPART (day,AttDate) when 9 then InMin else 0 end) as '9_InM',sum(case DATEPART (day,AttDate) when 9 then OutHour else 0 end) as '9_OutH',sum(case DATEPART (day,AttDate) when 9 then OutMin else 0 end) as '9_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 10 then InHour else 0 end) as '10_InH',sum(case DATEPART (day,AttDate) when 10 then InMin else 0 end) as '10_InM',sum(case DATEPART (day,AttDate) when 10 then OutHour else 0 end) as '10_OutH',sum(case DATEPART (day,AttDate) when 10 then OutMin else 0 end) as '10_OutM'," +

                     "sum(case DATEPART (day,AttDate) when 11 then InHour else 0 end) as '11_InH',sum(case DATEPART (day,AttDate) when 11 then InMin else 0 end) as '11_InM',sum(case DATEPART (day,AttDate) when 11 then OutHour else 0 end) as '11_OutH',sum(case DATEPART (day,AttDate) when 11 then OutMin else 0 end) as '11_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 12 then InHour else 0 end) as '12_InH',sum(case DATEPART (day,AttDate) when 12 then InMin else 0 end) as '12_InM',sum(case DATEPART (day,AttDate) when 12 then OutHour else 0 end) as '12_OutH',sum(case DATEPART (day,AttDate) when 12 then OutMin else 0 end) as '12_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 13 then InHour else 0 end) as '13_InH',sum(case DATEPART (day,AttDate) when 13 then InMin else 0 end) as '13_InM',sum(case DATEPART (day,AttDate) when 13 then OutHour else 0 end) as '13_OutH',sum(case DATEPART (day,AttDate) when 13 then OutMin else 0 end) as '13_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 14 then InHour else 0 end) as '14_InH',sum(case DATEPART (day,AttDate) when 14 then InMin else 0 end) as '14_InM',sum(case DATEPART (day,AttDate) when 14 then OutHour else 0 end) as '14_OutH',sum(case DATEPART (day,AttDate) when 14 then OutMin else 0 end) as '14_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 15 then InHour else 0 end) as '15_InH',sum(case DATEPART (day,AttDate) when 15 then InMin else 0 end) as '15_InM',sum(case DATEPART (day,AttDate) when 15 then OutHour else 0 end) as '15_OutH',sum(case DATEPART (day,AttDate) when 15 then OutMin else 0 end) as '15_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 16 then InHour else 0 end) as '16_InH',sum(case DATEPART (day,AttDate) when 16 then InMin else 0 end) as '16_InM',sum(case DATEPART (day,AttDate) when 16 then OutHour else 0 end) as '16_OutH',sum(case DATEPART (day,AttDate) when 16 then OutMin else 0 end) as '16_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 17 then InHour else 0 end) as '17_InH',sum(case DATEPART (day,AttDate) when 17 then InMin else 0 end) as '17_InM',sum(case DATEPART (day,AttDate) when 17 then OutHour else 0 end) as '17_OutH',sum(case DATEPART (day,AttDate) when 17 then OutMin else 0 end) as '17_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 18 then InHour else 0 end) as '18_InH',sum(case DATEPART (day,AttDate) when 18 then InMin else 0 end) as '18_InM',sum(case DATEPART (day,AttDate) when 18 then OutHour else 0 end) as '18_OutH',sum(case DATEPART (day,AttDate) when 18 then OutMin else 0 end) as '18_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 19 then InHour else 0 end) as '19_InH',sum(case DATEPART (day,AttDate) when 19 then InMin else 0 end) as '19_InM',sum(case DATEPART (day,AttDate) when 19 then OutHour else 0 end) as '19_OutH',sum(case DATEPART (day,AttDate) when 19 then OutMin else 0 end) as '19_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 20 then InHour else 0 end) as '20_InH',sum(case DATEPART (day,AttDate) when 20 then InMin else 0 end) as '20_InM',sum(case DATEPART (day,AttDate) when 20 then OutHour else 0 end) as '20_OutH',sum(case DATEPART (day,AttDate) when 20 then OutMin else 0 end) as '20_OutM'," +

                     "sum(case DATEPART (day,AttDate) when 21 then InHour else 0 end) as '21_InH',sum(case DATEPART (day,AttDate) when 21 then InMin else 0 end) as '21_InM',sum(case DATEPART (day,AttDate) when 21 then OutHour else 0 end) as '21_OutH',sum(case DATEPART (day,AttDate) when 21 then OutMin else 0 end) as '21_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 22 then InHour else 0 end) as '22_InH',sum(case DATEPART (day,AttDate) when 22 then InMin else 0 end) as '22_InM',sum(case DATEPART (day,AttDate) when 22 then OutHour else 0 end) as '22_OutH',sum(case DATEPART (day,AttDate) when 22 then OutMin else 0 end) as '22_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 23 then InHour else 0 end) as '23_InH',sum(case DATEPART (day,AttDate) when 23 then InMin else 0 end) as '23_InM',sum(case DATEPART (day,AttDate) when 23 then OutHour else 0 end) as '23_OutH',sum(case DATEPART (day,AttDate) when 23 then OutMin else 0 end) as '23_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 24 then InHour else 0 end) as '24_InH',sum(case DATEPART (day,AttDate) when 24 then InMin else 0 end) as '24_InM',sum(case DATEPART (day,AttDate) when 24 then OutHour else 0 end) as '24_OutH',sum(case DATEPART (day,AttDate) when 24 then OutMin else 0 end) as '24_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 25 then InHour else 0 end) as '25_InH',sum(case DATEPART (day,AttDate) when 25 then InMin else 0 end) as '25_InM',sum(case DATEPART (day,AttDate) when 25 then OutHour else 0 end) as '25_OutH',sum(case DATEPART (day,AttDate) when 25 then OutMin else 0 end) as '25_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 26 then InHour else 0 end) as '26_InH',sum(case DATEPART (day,AttDate) when 26 then InMin else 0 end) as '26_InM',sum(case DATEPART (day,AttDate) when 26 then OutHour else 0 end) as '26_OutH',sum(case DATEPART (day,AttDate) when 26 then OutMin else 0 end) as '26_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 27 then InHour else 0 end) as '27_InH',sum(case DATEPART (day,AttDate) when 27 then InMin else 0 end) as '27_InM',sum(case DATEPART (day,AttDate) when 27 then OutHour else 0 end) as '27_OutH',sum(case DATEPART (day,AttDate) when 27 then OutMin else 0 end) as '27_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 28 then InHour else 0 end) as '28_InH',sum(case DATEPART (day,AttDate) when 28 then InMin else 0 end) as '28_InM',sum(case DATEPART (day,AttDate) when 28 then OutHour else 0 end) as '28_OutH',sum(case DATEPART (day,AttDate) when 28 then OutMin else 0 end) as '28_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 29 then InHour else 0 end) as '29_InH',sum(case DATEPART (day,AttDate) when 29 then InMin else 0 end) as '29_InM',sum(case DATEPART (day,AttDate) when 29 then OutHour else 0 end) as '29_OutH',sum(case DATEPART (day,AttDate) when 29 then OutMin else 0 end) as '29_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 30 then InHour else 0 end) as '30_InH',sum(case DATEPART (day,AttDate) when 30 then InMin else 0 end) as '30_InM',sum(case DATEPART (day,AttDate) when 30 then OutHour else 0 end) as '30_OutH',sum(case DATEPART (day,AttDate) when 30 then OutMin else 0 end) as '30_OutM'," +
                     "sum(case DATEPART (day,AttDate) when 31 then InHour else 0 end) as '31_InH',sum(case DATEPART (day,AttDate) when 31 then InMin else 0 end) as '31_InM',sum(case DATEPART (day,AttDate) when 31 then OutHour else 0 end) as '31_OutH',sum(case DATEPART (day,AttDate) when 31 then OutMin else 0 end) as '31_OutM'" +
                     ",DptId,DptNameBn DptName,SftId,SftNameBangla SftName,CompanyNameBangla CompanyName,AddressBangla Address " +
                     "from v_tblAttendanceRecord " +
                     "Where CompanyId " + CompanyId + "   AND DptId " + DepartmentList + " " + EmpTypeID + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "'" +
                     "group by EmpCardNo, EmpId,EmpNameBn,DptId,DptNameBn,SftId,SftNameBangla,CompanyNameBangla,AddressBangla,convert(int,DptCode), convert(int,SftId),CustomOrdering " +
                             " order by convert(int,DptCode), convert(int,SftId),CustomOrdering";
                else
                    sql = "select Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo, EmpId,EmpNameBn EmpName,sum(case DATEPART (day,AttDate) when 1 then InHour else 0 end) as '1_InH',sum(case DATEPART (day,AttDate) when 1 then InMin else 0 end) as '1_InM',sum(case DATEPART (day,AttDate) when 1 then OutHour else 0 end) as '1_OutH',sum(case DATEPART (day,AttDate) when 1 then OutMin else 0 end) as '1_OutM'," +
                "sum(case DATEPART (day,AttDate) when 2 then InHour else 0 end) as '2_InH',sum(case DATEPART (day,AttDate) when 2 then InMin else 0 end) as '2_InM',sum(case DATEPART (day,AttDate) when 2 then OutHour else 0 end) as '2_OutH',sum(case DATEPART (day,AttDate) when 2 then OutMin else 0 end) as '2_OutM'," +
                "sum(case DATEPART (day,AttDate) when 3 then InHour else 0 end) as '3_InH',sum(case DATEPART (day,AttDate) when 3 then InMin else 0 end) as '3_InM',sum(case DATEPART (day,AttDate) when 3 then OutHour else 0 end) as '3_OutH',sum(case DATEPART (day,AttDate) when 3 then OutMin else 0 end) as '3_OutM'," +
                "sum(case DATEPART (day,AttDate) when 4 then InHour else 0 end) as '4_InH',sum(case DATEPART (day,AttDate) when 4 then InMin else 0 end) as '4_InM',sum(case DATEPART (day,AttDate) when 4 then OutHour else 0 end) as '4_OutH',sum(case DATEPART (day,AttDate) when 4 then OutMin else 0 end) as '4_OutM'," +
                "sum(case DATEPART (day,AttDate) when 5 then InHour else 0 end) as '5_InH',sum(case DATEPART (day,AttDate) when 5 then InMin else 0 end) as '5_InM',sum(case DATEPART (day,AttDate) when 5 then OutHour else 0 end) as '5_OutH',sum(case DATEPART (day,AttDate) when 5 then OutMin else 0 end) as '5_OutM'," +
                "sum(case DATEPART (day,AttDate) when 6 then InHour else 0 end) as '6_InH',sum(case DATEPART (day,AttDate) when 6 then InMin else 0 end) as '6_InM',sum(case DATEPART (day,AttDate) when 6 then OutHour else 0 end) as '6_OutH',sum(case DATEPART (day,AttDate) when 6 then OutMin else 0 end) as '6_OutM'," +
                "sum(case DATEPART (day,AttDate) when 7 then InHour else 0 end) as '7_InH',sum(case DATEPART (day,AttDate) when 7 then InMin else 0 end) as '7_InM',sum(case DATEPART (day,AttDate) when 7 then OutHour else 0 end) as '7_OutH',sum(case DATEPART (day,AttDate) when 7 then OutMin else 0 end) as '7_OutM'," +
                "sum(case DATEPART (day,AttDate) when 8 then InHour else 0 end) as '8_InH',sum(case DATEPART (day,AttDate) when 8 then InMin else 0 end) as '8_InM',sum(case DATEPART (day,AttDate) when 8 then OutHour else 0 end) as '8_OutH',sum(case DATEPART (day,AttDate) when 8 then OutMin else 0 end) as '8_OutM'," +
                "sum(case DATEPART (day,AttDate) when 9 then InHour else 0 end) as '9_InH',sum(case DATEPART (day,AttDate) when 9 then InMin else 0 end) as '9_InM',sum(case DATEPART (day,AttDate) when 9 then OutHour else 0 end) as '9_OutH',sum(case DATEPART (day,AttDate) when 9 then OutMin else 0 end) as '9_OutM'," +
                "sum(case DATEPART (day,AttDate) when 10 then InHour else 0 end) as '10_InH',sum(case DATEPART (day,AttDate) when 10 then InMin else 0 end) as '10_InM',sum(case DATEPART (day,AttDate) when 10 then OutHour else 0 end) as '10_OutH',sum(case DATEPART (day,AttDate) when 10 then OutMin else 0 end) as '10_OutM'," +

                "sum(case DATEPART (day,AttDate) when 11 then InHour else 0 end) as '11_InH',sum(case DATEPART (day,AttDate) when 11 then InMin else 0 end) as '11_InM',sum(case DATEPART (day,AttDate) when 11 then OutHour else 0 end) as '11_OutH',sum(case DATEPART (day,AttDate) when 11 then OutMin else 0 end) as '11_OutM'," +
                "sum(case DATEPART (day,AttDate) when 12 then InHour else 0 end) as '12_InH',sum(case DATEPART (day,AttDate) when 12 then InMin else 0 end) as '12_InM',sum(case DATEPART (day,AttDate) when 12 then OutHour else 0 end) as '12_OutH',sum(case DATEPART (day,AttDate) when 12 then OutMin else 0 end) as '12_OutM'," +
                "sum(case DATEPART (day,AttDate) when 13 then InHour else 0 end) as '13_InH',sum(case DATEPART (day,AttDate) when 13 then InMin else 0 end) as '13_InM',sum(case DATEPART (day,AttDate) when 13 then OutHour else 0 end) as '13_OutH',sum(case DATEPART (day,AttDate) when 13 then OutMin else 0 end) as '13_OutM'," +
                "sum(case DATEPART (day,AttDate) when 14 then InHour else 0 end) as '14_InH',sum(case DATEPART (day,AttDate) when 14 then InMin else 0 end) as '14_InM',sum(case DATEPART (day,AttDate) when 14 then OutHour else 0 end) as '14_OutH',sum(case DATEPART (day,AttDate) when 14 then OutMin else 0 end) as '14_OutM'," +
                "sum(case DATEPART (day,AttDate) when 15 then InHour else 0 end) as '15_InH',sum(case DATEPART (day,AttDate) when 15 then InMin else 0 end) as '15_InM',sum(case DATEPART (day,AttDate) when 15 then OutHour else 0 end) as '15_OutH',sum(case DATEPART (day,AttDate) when 15 then OutMin else 0 end) as '15_OutM'," +
                "sum(case DATEPART (day,AttDate) when 16 then InHour else 0 end) as '16_InH',sum(case DATEPART (day,AttDate) when 16 then InMin else 0 end) as '16_InM',sum(case DATEPART (day,AttDate) when 16 then OutHour else 0 end) as '16_OutH',sum(case DATEPART (day,AttDate) when 16 then OutMin else 0 end) as '16_OutM'," +
                "sum(case DATEPART (day,AttDate) when 17 then InHour else 0 end) as '17_InH',sum(case DATEPART (day,AttDate) when 17 then InMin else 0 end) as '17_InM',sum(case DATEPART (day,AttDate) when 17 then OutHour else 0 end) as '17_OutH',sum(case DATEPART (day,AttDate) when 17 then OutMin else 0 end) as '17_OutM'," +
                "sum(case DATEPART (day,AttDate) when 18 then InHour else 0 end) as '18_InH',sum(case DATEPART (day,AttDate) when 18 then InMin else 0 end) as '18_InM',sum(case DATEPART (day,AttDate) when 18 then OutHour else 0 end) as '18_OutH',sum(case DATEPART (day,AttDate) when 18 then OutMin else 0 end) as '18_OutM'," +
                "sum(case DATEPART (day,AttDate) when 19 then InHour else 0 end) as '19_InH',sum(case DATEPART (day,AttDate) when 19 then InMin else 0 end) as '19_InM',sum(case DATEPART (day,AttDate) when 19 then OutHour else 0 end) as '19_OutH',sum(case DATEPART (day,AttDate) when 19 then OutMin else 0 end) as '19_OutM'," +
                "sum(case DATEPART (day,AttDate) when 20 then InHour else 0 end) as '20_InH',sum(case DATEPART (day,AttDate) when 20 then InMin else 0 end) as '20_InM',sum(case DATEPART (day,AttDate) when 20 then OutHour else 0 end) as '20_OutH',sum(case DATEPART (day,AttDate) when 20 then OutMin else 0 end) as '20_OutM'," +

                "sum(case DATEPART (day,AttDate) when 21 then InHour else 0 end) as '21_InH',sum(case DATEPART (day,AttDate) when 21 then InMin else 0 end) as '21_InM',sum(case DATEPART (day,AttDate) when 21 then OutHour else 0 end) as '21_OutH',sum(case DATEPART (day,AttDate) when 21 then OutMin else 0 end) as '21_OutM'," +
                "sum(case DATEPART (day,AttDate) when 22 then InHour else 0 end) as '22_InH',sum(case DATEPART (day,AttDate) when 22 then InMin else 0 end) as '22_InM',sum(case DATEPART (day,AttDate) when 22 then OutHour else 0 end) as '22_OutH',sum(case DATEPART (day,AttDate) when 22 then OutMin else 0 end) as '22_OutM'," +
                "sum(case DATEPART (day,AttDate) when 23 then InHour else 0 end) as '23_InH',sum(case DATEPART (day,AttDate) when 23 then InMin else 0 end) as '23_InM',sum(case DATEPART (day,AttDate) when 23 then OutHour else 0 end) as '23_OutH',sum(case DATEPART (day,AttDate) when 23 then OutMin else 0 end) as '23_OutM'," +
                "sum(case DATEPART (day,AttDate) when 24 then InHour else 0 end) as '24_InH',sum(case DATEPART (day,AttDate) when 24 then InMin else 0 end) as '24_InM',sum(case DATEPART (day,AttDate) when 24 then OutHour else 0 end) as '24_OutH',sum(case DATEPART (day,AttDate) when 24 then OutMin else 0 end) as '24_OutM'," +
                "sum(case DATEPART (day,AttDate) when 25 then InHour else 0 end) as '25_InH',sum(case DATEPART (day,AttDate) when 25 then InMin else 0 end) as '25_InM',sum(case DATEPART (day,AttDate) when 25 then OutHour else 0 end) as '25_OutH',sum(case DATEPART (day,AttDate) when 25 then OutMin else 0 end) as '25_OutM'," +
                "sum(case DATEPART (day,AttDate) when 26 then InHour else 0 end) as '26_InH',sum(case DATEPART (day,AttDate) when 26 then InMin else 0 end) as '26_InM',sum(case DATEPART (day,AttDate) when 26 then OutHour else 0 end) as '26_OutH',sum(case DATEPART (day,AttDate) when 26 then OutMin else 0 end) as '26_OutM'," +
                "sum(case DATEPART (day,AttDate) when 27 then InHour else 0 end) as '27_InH',sum(case DATEPART (day,AttDate) when 27 then InMin else 0 end) as '27_InM',sum(case DATEPART (day,AttDate) when 27 then OutHour else 0 end) as '27_OutH',sum(case DATEPART (day,AttDate) when 27 then OutMin else 0 end) as '27_OutM'," +
                "sum(case DATEPART (day,AttDate) when 28 then InHour else 0 end) as '28_InH',sum(case DATEPART (day,AttDate) when 28 then InMin else 0 end) as '28_InM',sum(case DATEPART (day,AttDate) when 28 then OutHour else 0 end) as '28_OutH',sum(case DATEPART (day,AttDate) when 28 then OutMin else 0 end) as '28_OutM'," +
                "sum(case DATEPART (day,AttDate) when 29 then InHour else 0 end) as '29_InH',sum(case DATEPART (day,AttDate) when 29 then InMin else 0 end) as '29_InM',sum(case DATEPART (day,AttDate) when 29 then OutHour else 0 end) as '29_OutH',sum(case DATEPART (day,AttDate) when 29 then OutMin else 0 end) as '29_OutM'," +
                "sum(case DATEPART (day,AttDate) when 30 then InHour else 0 end) as '30_InH',sum(case DATEPART (day,AttDate) when 30 then InMin else 0 end) as '30_InM',sum(case DATEPART (day,AttDate) when 30 then OutHour else 0 end) as '30_OutH',sum(case DATEPART (day,AttDate) when 30 then OutMin else 0 end) as '30_OutM'," +
                "sum(case DATEPART (day,AttDate) when 31 then InHour else 0 end) as '31_InH',sum(case DATEPART (day,AttDate) when 31 then InMin else 0 end) as '31_InM',sum(case DATEPART (day,AttDate) when 31 then OutHour else 0 end) as '31_OutH',sum(case DATEPART (day,AttDate) when 31 then OutMin else 0 end) as '31_OutM'" +
                ",DptId,DptNameBn DptName,SftId,SftNameBangla SftName,CompanyNameBangla CompanyName,AddressBangla Address " +
                "from v_tblAttendanceRecord " +
                "Where CompanyId " + CompanyId + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "' AND Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + EmpCardNo + "'" +
                "group by EmpCardNo, EmpId,EmpName,DptId,DptNameBn,SftId,SftNameBangla,CompanyNameBangla,AddressBangla ";

                sqlDB.fillDataTable(sql, dt = new DataTable());
                return dt;
            }
            catch { return null; }
        }


        public static DataTable get_Moanthly_Attendance_SheetBangla(string CompanyId, string DepartmentList, string Month, string Year, int index, string EmpCardNo, string EmpTypeID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (index == 0)
                    sql = "select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpNameBn EmpName," +
                        "sum(case DATEPART (day,AttDate) when 1 then code else 0 end) as '1'," +
                        "sum(case DATEPART (day,AttDate) when 2 then code else 0 end) as '2'," +
                        "sum(case DATEPART (day,AttDate) when 3 then code else 0 end) as '3'," +
                        "sum(case DATEPART (day,AttDate) when 4 then code else 0 end) as '4'," +
                        "sum(case DATEPART (day,AttDate) when 5 then code else 0 end) as '5'," +
                        "sum(case DATEPART (day,AttDate) when 6 then code else 0 end) as '6'," +
                        "sum(case DATEPART (day,AttDate) when 7 then code else 0 end) as '7'," +
                        "sum(case DATEPART (day,AttDate) when 8 then code else 0 end) as '8'," +
                        "sum(case DATEPART (day,AttDate) when 9 then code else 0 end) as '9'," +
                        "sum(case DATEPART (day,AttDate) when 10 then code else 0 end) as '10'," +
                        "sum(case DATEPART (day,AttDate) when 11 then code else 0 end) as '11'," +
                        "sum(case DATEPART (day,AttDate) when 12 then code else 0 end) as '12'," +
                        "sum(case DATEPART (day,AttDate) when 13 then code else 0 end) as '13'," +
                        "sum(case DATEPART (day,AttDate) when 14 then code else 0 end) as '14'," +
                        "sum(case DATEPART (day,AttDate) when 15 then code else 0 end) as '15'," +
                        "sum(case DATEPART (day,AttDate) when 16 then code else 0 end) as '1'," +
                        "sum(case DATEPART (day,AttDate) when 17 then code else 0 end) as '17'," +
                        "sum(case DATEPART (day,AttDate) when 18 then code else 0 end) as '18'," +
                        "sum(case DATEPART (day,AttDate) when 19 then code else 0 end) as '19'," +
                        "sum(case DATEPART (day,AttDate) when 20 then code else 0 end) as '20'," +
                        "sum(case DATEPART (day,AttDate) when 21 then code else 0 end) as '21'," +
                        "sum(case DATEPART (day,AttDate) when 22 then code else 0 end) as '22'," +
                        "sum(case DATEPART (day,AttDate) when 23 then code else 0 end) as '23'," +
                        "sum(case DATEPART (day,AttDate) when 24 then code else 0 end) as '24'," +
                        "sum(case DATEPART (day,AttDate) when 25 then code else 0 end) as '25'," +
                        "sum(case DATEPART (day,AttDate) when 26 then code else 0 end) as '26'," +
                        "sum(case DATEPART (day,AttDate) when 27 then code else 0 end) as '27'," +
                        "sum(case DATEPART (day,AttDate) when 28 then code else 0 end) as '28'," +
                        "sum(case DATEPART (day,AttDate) when 29 then code else 0 end) as '29'," +
                        "sum(case DATEPART (day,AttDate) when 30 then code else 0 end) as '30'," +
                        "sum(case DATEPART (day,AttDate) when 31 then code else 0 end) as '31'," +
                        " DptId,DptNameBn DptName,DsgNameBn DsgName,SftId,SftNameBangla SftName,CompanyId,CompanyNameBangla CompanyName,AddressBangla Address " +
                        "from v_tblAttendanceRecord " +
                        "Where CompanyId " + CompanyId + "  AND DptId " + DepartmentList + " " + EmpTypeID + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "'" +
                        "group by EmpId,EmpCardNo,EmpNameBn,DptId,DptNameBn,DsgNameBn,SftId,SftNameBangla,CompanyId,CompanyNameBangla,AddressBangla,convert(int,DptCode), convert(int,SftId),CustomOrdering " +
                            " order by convert(int,DptCode), convert(int,SftId),CustomOrdering";
                else
                    sql = "select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpNameBn EmpName," +
                    "sum(case DATEPART (day,AttDate) when 1 then code else 0 end) as '1'," +
                    "sum(case DATEPART (day,AttDate) when 2 then code else 0 end) as '2'," +
                    "sum(case DATEPART (day,AttDate) when 3 then code else 0 end) as '3'," +
                    "sum(case DATEPART (day,AttDate) when 4 then code else 0 end) as '4'," +
                    "sum(case DATEPART (day,AttDate) when 5 then code else 0 end) as '5'," +
                    "sum(case DATEPART (day,AttDate) when 6 then code else 0 end) as '6'," +
                    "sum(case DATEPART (day,AttDate) when 7 then code else 0 end) as '7'," +
                    "sum(case DATEPART (day,AttDate) when 8 then code else 0 end) as '8'," +
                    "sum(case DATEPART (day,AttDate) when 9 then code else 0 end) as '9'," +
                    "sum(case DATEPART (day,AttDate) when 10 then code else 0 end) as '10'," +
                    "sum(case DATEPART (day,AttDate) when 11 then code else 0 end) as '11'," +
                    "sum(case DATEPART (day,AttDate) when 12 then code else 0 end) as '12'," +
                    "sum(case DATEPART (day,AttDate) when 13 then code else 0 end) as '13'," +
                    "sum(case DATEPART (day,AttDate) when 14 then code else 0 end) as '14'," +
                    "sum(case DATEPART (day,AttDate) when 15 then code else 0 end) as '15'," +
                    "sum(case DATEPART (day,AttDate) when 16 then code else 0 end) as '1'," +
                    "sum(case DATEPART (day,AttDate) when 17 then code else 0 end) as '17'," +
                    "sum(case DATEPART (day,AttDate) when 18 then code else 0 end) as '18'," +
                    "sum(case DATEPART (day,AttDate) when 19 then code else 0 end) as '19'," +
                    "sum(case DATEPART (day,AttDate) when 20 then code else 0 end) as '20'," +
                    "sum(case DATEPART (day,AttDate) when 21 then code else 0 end) as '21'," +
                    "sum(case DATEPART (day,AttDate) when 22 then code else 0 end) as '22'," +
                    "sum(case DATEPART (day,AttDate) when 23 then code else 0 end) as '23'," +
                    "sum(case DATEPART (day,AttDate) when 24 then code else 0 end) as '24'," +
                    "sum(case DATEPART (day,AttDate) when 25 then code else 0 end) as '25'," +
                    "sum(case DATEPART (day,AttDate) when 26 then code else 0 end) as '26'," +
                    "sum(case DATEPART (day,AttDate) when 27 then code else 0 end) as '27'," +
                    "sum(case DATEPART (day,AttDate) when 28 then code else 0 end) as '28'," +
                    "sum(case DATEPART (day,AttDate) when 29 then code else 0 end) as '29'," +
                    "sum(case DATEPART (day,AttDate) when 30 then code else 0 end) as '30'," +
                    "sum(case DATEPART (day,AttDate) when 31 then code else 0 end) as '31'," +
                    " DsgNameBn DsgName,DptId,DptNameBn DptName,SftId,SftNameBangla SftName,CompanyId,CompanyNameBangla CompanyName,AddressBangla Address " +
                    "from v_tblAttendanceRecord " +
                    "Where CompanyId " + CompanyId + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "' AND Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + EmpCardNo + "'" +
                    " group by EmpId,EmpCardNo,EmpNameBn,DsgNameBn,DptId,DptNameBn,SftId,SftNameBangla,CompanyId,CompanyNameBangla,AddressBangla ";

                sqlDB.fillDataTable(sql, dt = new DataTable());
                return dt;
            }
            catch { return null; }
        }

        public static DataTable get_Moanthly_Attendance_Sheet_SummaryBangla(string CompanyId, string DepartmentList, string Month, string Year, int index, string EmpCardNo, string EmpTypeID)
        {
            try
            {
                DataTable dt = new DataTable();
                if (index == 0)
                    sql = "SELECT        EmpId, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpNameBn EmpName,AddressBangla Address, SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN code ELSE 0 END) AS [1], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN code ELSE 0 END) AS [2]," +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN code ELSE 0 END) AS [3], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN code ELSE 0 END) AS [4], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 5 THEN code ELSE 0 END) AS [5], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN code ELSE 0 END) AS [6], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN code ELSE 0 END) AS [7]," +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN code ELSE 0 END) AS [8], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN code ELSE 0 END) AS [9], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 10 THEN code ELSE 0 END) AS [10], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN code ELSE 0 END) AS [11], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN code ELSE 0 END) AS [12]," +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN code ELSE 0 END) AS [13], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN code ELSE 0 END) AS [14], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 15 THEN code ELSE 0 END) AS [15], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN code ELSE 0 END) AS [16], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN code ELSE 0 END) AS [17], " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN code ELSE 0 END) AS [18], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN code ELSE 0 END) AS [19], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 20 THEN code ELSE 0 END) AS [20], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN code ELSE 0 END) AS [21], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN code ELSE 0 END) AS [22], " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN code ELSE 0 END) AS [23], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN code ELSE 0 END) AS [24], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 25 THEN code ELSE 0 END) AS [25], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN code ELSE 0 END) AS [26], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN code ELSE 0 END) AS [27], " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN code ELSE 0 END) AS [28], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN code ELSE 0 END) AS [29], SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 30 THEN code ELSE 0 END) AS [30], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN code ELSE 0 END) AS [31],DsgNameBn DsgName, DptId,DptNameBn DptName, SftId, SftNameBangla SftName, CompanyId,CompanyNameBangla CompanyName," +
                          "(SUM(case Code when 112 then 1 else 0 end)+SUM(case Code when 108 then 1 else 0 end)) as P,SUM(case Code when 97 then 1 else 0 end) as A ,SUM(case Code when 108 then 1 else 0 end) as L,SUM(case Code when 104  then 1 else 0 end) as H,SUM(case Code when 119  then 1 else 0 end) as W," +
                          "SUM(case Code when 226  then 1 else 0 end) as LV," +

                          "SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN OverTime ELSE 0 END) AS '1_OT', SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN OverTime ELSE 0 END) AS '2_OT', " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN OverTime ELSE 0 END) AS '3_OT', SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN OverTime ELSE 0 END) AS '4_OT', SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 5 THEN OverTime ELSE 0 END) AS '5_OT', SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN OverTime ELSE 0 END) AS '6_OT', SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN OverTime ELSE 0 END) AS '7_OT', " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN OverTime ELSE 0 END) AS '8_OT', SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN OverTime ELSE 0 END) AS '9_OT', SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 10 THEN OverTime ELSE 0 END) AS '10_OT', SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN OverTime ELSE 0 END) AS '11_OT', SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN OverTime ELSE 0 END) AS '12_OT', " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN OverTime ELSE 0 END) AS '13_OT', SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN OverTime ELSE 0 END) AS '14_OT' , SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 15 THEN OverTime ELSE 0 END) AS '15_OT', SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN OverTime ELSE 0 END) AS '16_OT', SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN OverTime ELSE 0 END) AS '17_OT', " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN OverTime ELSE 0 END) AS '18_OT', SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN OverTime ELSE 0 END) AS '19_OT', SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 20 THEN OverTime ELSE 0 END) AS '20_OT', SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN OverTime ELSE 0 END) AS '21_OT', SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN OverTime ELSE 0 END) AS '22_OT', " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN OverTime ELSE 0 END) AS '23_OT', SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN OverTime ELSE 0 END) AS '24_OT', SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 25 THEN OverTime ELSE 0 END) AS '25_OT', SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN OverTime ELSE 0 END) AS '26_OT', SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN OverTime ELSE 0 END) AS '27_OT', " +
                          "SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN OverTime ELSE 0 END) AS '28_OT', SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN OverTime ELSE 0 END) AS '29_OT', SUM(CASE DATEPART(day, AttDate) " +
                          "WHEN 30 THEN OverTime ELSE 0 END) AS '30_OT', SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN OverTime ELSE 0 END) AS '31_OT', SUM(OverTime) AS MonthlyTotalOT " +
                          "   FROM            dbo.v_tblAttendanceRecord " +
                          " Where CompanyId " + CompanyId + " AND DptId " + DepartmentList + " " + EmpTypeID + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "'" +
                          "   GROUP BY EmpId, EmpCardNo, EmpNameBn,DsgNameBn, DptId, DptNameBn, SftId, SftNameBangla, CompanyId, CompanyNameBangla,AddressBangla,convert(int,DptCode), convert(int,SftId),CustomOrdering " +
                         " order by convert(int,DptCode), convert(int,SftId),CustomOrdering";
                else
                    sql = "SELECT        EmpId, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpNameBn EmpName,AddressBangla Address, SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN code ELSE 0 END) AS [1], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN code ELSE 0 END) AS [2]," +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN code ELSE 0 END) AS [3], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN code ELSE 0 END) AS [4], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 5 THEN code ELSE 0 END) AS [5], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN code ELSE 0 END) AS [6], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN code ELSE 0 END) AS [7]," +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN code ELSE 0 END) AS [8], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN code ELSE 0 END) AS [9], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 10 THEN code ELSE 0 END) AS [10], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN code ELSE 0 END) AS [11], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN code ELSE 0 END) AS [12]," +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN code ELSE 0 END) AS [13], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN code ELSE 0 END) AS [14], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 15 THEN code ELSE 0 END) AS [15], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN code ELSE 0 END) AS [16], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN code ELSE 0 END) AS [17], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN code ELSE 0 END) AS [18], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN code ELSE 0 END) AS [19], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 20 THEN code ELSE 0 END) AS [20], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN code ELSE 0 END) AS [21], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN code ELSE 0 END) AS [22], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN code ELSE 0 END) AS [23], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN code ELSE 0 END) AS [24], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 25 THEN code ELSE 0 END) AS [25], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN code ELSE 0 END) AS [26], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN code ELSE 0 END) AS [27], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN code ELSE 0 END) AS [28], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN code ELSE 0 END) AS [29], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 30 THEN code ELSE 0 END) AS [30], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN code ELSE 0 END) AS [31],DsgNameBn DsgName, DptId,DptNameBn DptName, SftId,SftNameBangla SftName, CompanyId,CompanyNameBangla CompanyName," +
                        "(SUM(case Code when 112 then 1 else 0 end)+SUM(case Code when 108 then 1 else 0 end)) as P,SUM(case Code when 97 then 1 else 0 end) as A ,SUM(case Code when 108 then 1 else 0 end) as L,SUM(case Code when 104  then 1 else 0 end) as H,SUM(case Code when 119  then 1 else 0 end) as W," +
                        "SUM(case Code when 226  then 1 else 0 end) as LV," +

                        "SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN OverTime ELSE 0 END) AS '1_OT', SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN OverTime ELSE 0 END) AS '2_OT', " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN OverTime ELSE 0 END) AS '3_OT', SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN OverTime ELSE 0 END) AS '4_OT', SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 5 THEN OverTime ELSE 0 END) AS '5_OT', SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN OverTime ELSE 0 END) AS '6_OT', SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN OverTime ELSE 0 END) AS '7_OT', " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN OverTime ELSE 0 END) AS '8_OT', SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN OverTime ELSE 0 END) AS '9_OT', SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 10 THEN OverTime ELSE 0 END) AS '10_OT', SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN OverTime ELSE 0 END) AS '11_OT', SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN OverTime ELSE 0 END) AS '12_OT', " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN OverTime ELSE 0 END) AS '13_OT', SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN OverTime ELSE 0 END) AS '14_OT' , SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 15 THEN OverTime ELSE 0 END) AS '15_OT', SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN OverTime ELSE 0 END) AS '16_OT', SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN OverTime ELSE 0 END) AS '17_OT', " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN OverTime ELSE 0 END) AS '18_OT', SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN OverTime ELSE 0 END) AS '19_OT', SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 20 THEN OverTime ELSE 0 END) AS '20_OT', SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN OverTime ELSE 0 END) AS '21_OT', SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN OverTime ELSE 0 END) AS '22_OT', " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN OverTime ELSE 0 END) AS '23_OT', SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN OverTime ELSE 0 END) AS '24_OT', SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 25 THEN OverTime ELSE 0 END) AS '25_OT', SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN OverTime ELSE 0 END) AS '26_OT', SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN OverTime ELSE 0 END) AS '27_OT', " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN OverTime ELSE 0 END) AS '28_OT', SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN OverTime ELSE 0 END) AS '29_OT', SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 30 THEN OverTime ELSE 0 END) AS '30_OT', SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN OverTime ELSE 0 END) AS '31_OT', SUM(OverTime) AS MonthlyTotalOT " +
                        "   FROM            dbo.v_tblAttendanceRecord " +
                        "Where CompanyId " + CompanyId + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "' AND Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + EmpCardNo + "'" +
                        "   GROUP BY EmpId, EmpCardNo, EmpNameBn,DsgNameBn, DptId, DptNameBn, SftId, SftNameBangla, CompanyId, CompanyNameBangla,AddressBangla";

                sqlDB.fillDataTable(sql, dt);
                return dt;
            }
            catch { return null; }
        }
        //------------------------------------- Monthly Overtime Reprot--------------------------------------------
        public static DataTable get_MonthlyOvertimeReprot(string CompanyId, string DepartmentList, string ShiftList, string Month, string Year,string EmpCardNo, string EmptypeId)
        {
            try
            {

                DataTable dt = new DataTable();
                if (EmpCardNo.Trim().Length == 0) // 0 Means All
                    sql = "select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName, SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN InHour ELSE 0 END) AS [1_InH], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 1 THEN InMin ELSE 0 END) AS [1_InM], SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN OutHour ELSE 0 END) AS [1_OutH], SUM(CASE DATEPART(day, " +
                        "AttDate) WHEN 1 THEN OutMin ELSE 0 END) AS [1_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN CONVERT(int, datepart(hour, TotalOverTime)) " +
                        "ELSE 0 END) AS [1_STH], SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [1_STM], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN InHour ELSE 0 END) AS [2_InH], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN InMin ELSE 0 END) " +
                        "AS [2_InM], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN OutHour ELSE 0 END) AS [2_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 2 THEN OutMin ELSE 0 END) AS [2_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        "AS [2_STH], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [2_STM], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN InHour ELSE 0 END) AS [3_InH], SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN InMin ELSE 0 END) " +
                        "AS [3_InM], SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN OutHour ELSE 0 END) AS [3_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 3 THEN OutMin ELSE 0 END) AS [3_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        "AS [3_STH], SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [3_STM], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN InHour ELSE 0 END) AS [4_InH], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN InMin ELSE 0 END) " +
                        "AS [4_InM], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN OutHour ELSE 0 END) AS [4_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 4 THEN OutMin ELSE 0 END) AS [4_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        "AS [4_STH], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [4_STM], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 5 THEN InHour ELSE 0 END) AS [5_InH], SUM(CASE DATEPART(day, AttDate) WHEN 5 THEN InMin ELSE 0 END) " +
                        "AS [5_InM], SUM(CASE DATEPART(day, AttDate) WHEN 5 THEN OutHour ELSE 0 END) AS [5_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 5 THEN OutMin ELSE 0 END) AS [5_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 5 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        "AS [5_STH], SUM(CASE DATEPART(day, AttDate) WHEN 5 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [5_STM], " +
                        " SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN InHour ELSE 0 END) AS [6_InH], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN InMin ELSE 0 END) " +
                        "AS [6_InM], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN OutHour ELSE 0 END) AS [6_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        " WHEN 6 THEN OutMin ELSE 0 END) AS [6_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        "AS [6_STH], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [6_STM], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN InHour ELSE 0 END) AS [7_InH], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN InMin ELSE 0 END) " +
                        "AS [7_InM], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN OutHour ELSE 0 END) AS [7_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        " WHEN 7 THEN OutMin ELSE 0 END) AS [7_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        " AS [7_STH], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [7_STM], " +
                        " SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN InHour ELSE 0 END) AS [8_InH], SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN InMin ELSE 0 END) " +
                        " AS [8_InM], SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN OutHour ELSE 0 END) AS [8_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        " WHEN 8 THEN OutMin ELSE 0 END) AS [8_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [8_STH], SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [8_STM], " +
                         " SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN InHour ELSE 0 END) AS [9_InH], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN InMin ELSE 0 END) " +
                         " AS [9_InM], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN OutHour ELSE 0 END) AS [9_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         " WHEN 9 THEN OutMin ELSE 0 END) AS [9_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         " AS [9_STH], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [9_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 10 THEN InHour ELSE 0 END) AS [10_InH], SUM(CASE DATEPART(day, AttDate) WHEN 10 THEN InMin ELSE 0 END) " +
                         "AS [10_InM], SUM(CASE DATEPART(day, AttDate) WHEN 10 THEN OutHour ELSE 0 END) AS [10_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        " WHEN 10 THEN OutMin ELSE 0 END) AS [10_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 10 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [10_STH], SUM(CASE DATEPART(day, AttDate) WHEN 10 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [10_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN InHour ELSE 0 END) AS [11_InH], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN InMin ELSE 0 END) " +
                         "AS [11_InM], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN OutHour ELSE 0 END) AS [11_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 11 THEN OutMin ELSE 0 END) AS [11_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [11_STH], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [11_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN InHour ELSE 0 END) AS [12_InH], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN InMin ELSE 0 END) " +
                         "AS [12_InM], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN OutHour ELSE 0 END) AS [12_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 12 THEN OutMin ELSE 0 END) AS [12_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [12_STH], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [12_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN InHour ELSE 0 END) AS [13_InH], SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN InMin ELSE 0 END) " +
                         "AS [13_InM], SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN OutHour ELSE 0 END) AS [13_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 13 THEN OutMin ELSE 0 END) AS [13_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [13_STH], SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [13_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN InHour ELSE 0 END) AS [14_InH], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN InMin ELSE 0 END) " +
                         "AS [14_InM], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN OutHour ELSE 0 END) AS [14_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 14 THEN OutMin ELSE 0 END) AS [14_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [14_STH], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [14_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 15 THEN InHour ELSE 0 END) AS [15_InH], SUM(CASE DATEPART(day, AttDate) WHEN 15 THEN InMin ELSE 0 END) " +
                         "AS [15_InM], SUM(CASE DATEPART(day, AttDate) WHEN 15 THEN OutHour ELSE 0 END) AS [15_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 15 THEN OutMin ELSE 0 END) AS [15_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 15 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [15_STH], SUM(CASE DATEPART(day, AttDate) WHEN 15 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [15_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN InHour ELSE 0 END) AS [16_InH], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN InMin ELSE 0 END) " +
                         "AS [16_InM], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN OutHour ELSE 0 END) AS [16_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 16 THEN OutMin ELSE 0 END) AS [16_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [16_STH], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [16_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN InHour ELSE 0 END) AS [17_InH], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN InMin ELSE 0 END) " +
                         "AS [17_InM], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN OutHour ELSE 0 END) AS [17_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 17 THEN OutMin ELSE 0 END) AS [17_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [17_STH], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [17_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN InHour ELSE 0 END) AS [18_InH], SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN InMin ELSE 0 END) " +
                         "AS [18_InM], SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN OutHour ELSE 0 END) AS [18_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 18 THEN OutMin ELSE 0 END) AS [18_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [18_STH], SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [18_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN InHour ELSE 0 END) AS [19_InH], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN InMin ELSE 0 END) " +
                         "AS [19_InM], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN OutHour ELSE 0 END) AS [19_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 19 THEN OutMin ELSE 0 END) AS [19_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [19_STH], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [19_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 20 THEN InHour ELSE 0 END) AS [20_InH], SUM(CASE DATEPART(day, AttDate) WHEN 20 THEN InMin ELSE 0 END) " +
                         "AS [20_InM], SUM(CASE DATEPART(day, AttDate) WHEN 20 THEN OutHour ELSE 0 END) AS [20_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 20 THEN OutMin ELSE 0 END) AS [20_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 20 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [20_STH], SUM(CASE DATEPART(day, AttDate) WHEN 20 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [20_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN InHour ELSE 0 END) AS [21_InH], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN InMin ELSE 0 END) " +
                         "AS [21_InM], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN OutHour ELSE 0 END) AS [21_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 21 THEN OutMin ELSE 0 END) AS [21_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [21_STH], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [21_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN InHour ELSE 0 END) AS [22_InH], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN InMin ELSE 0 END) " +
                         "AS [22_InM], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN OutHour ELSE 0 END) AS [22_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 22 THEN OutMin ELSE 0 END) AS [22_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [22_STH], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [22_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN InHour ELSE 0 END) AS [23_InH], SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN InMin ELSE 0 END) " +
                         "AS [23_InM], SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN OutHour ELSE 0 END) AS [23_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 23 THEN OutMin ELSE 0 END) AS [23_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [23_STH], SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [23_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN InHour ELSE 0 END) AS [24_InH], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN InMin ELSE 0 END) " +
                         "AS [24_InM], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN OutHour ELSE 0 END) AS [24_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 24 THEN OutMin ELSE 0 END) AS [24_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [24_STH], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [24_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 25 THEN InHour ELSE 0 END) AS [25_InH], SUM(CASE DATEPART(day, AttDate) WHEN 25 THEN InMin ELSE 0 END) " +
                         "AS [25_InM], SUM(CASE DATEPART(day, AttDate) WHEN 25 THEN OutHour ELSE 0 END) AS [25_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 25 THEN OutMin ELSE 0 END) AS [25_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 25 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [25_STH], SUM(CASE DATEPART(day, AttDate) WHEN 25 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [25_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN InHour ELSE 0 END) AS [26_InH], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN InMin ELSE 0 END) " +
                         "AS [26_InM], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN OutHour ELSE 0 END) AS [26_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 26 THEN OutMin ELSE 0 END) AS [26_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [26_STH], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [26_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN InHour ELSE 0 END) AS [27_InH], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN InMin ELSE 0 END) " +
                         "AS [27_InM], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN OutHour ELSE 0 END) AS [27_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 27 THEN OutMin ELSE 0 END) AS [27_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [27_STH], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [27_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN InHour ELSE 0 END) AS [28_InH], SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN InMin ELSE 0 END) " +
                         "AS [28_InM], SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN OutHour ELSE 0 END) AS [28_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 28 THEN OutMin ELSE 0 END) AS [28_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [28_STH], SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [28_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN InHour ELSE 0 END) AS [29_InH], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN InMin ELSE 0 END) " +
                         "AS [29_InM], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN OutHour ELSE 0 END) AS [29_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 29 THEN OutMin ELSE 0 END) AS [29_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [29_STH], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [29_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 30 THEN InHour ELSE 0 END) AS [30_InH], SUM(CASE DATEPART(day, AttDate) WHEN 30 THEN InMin ELSE 0 END) " +
                         "AS [30_InM], SUM(CASE DATEPART(day, AttDate) WHEN 30 THEN OutHour ELSE 0 END) AS [30_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 30 THEN OutMin ELSE 0 END) AS [30_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 30 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [30_STH], SUM(CASE DATEPART(day, AttDate) WHEN 30 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [30_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN InHour ELSE 0 END) AS [31_InH], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN InMin ELSE 0 END) " +
                         "AS [31_InM], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN OutHour ELSE 0 END) AS [31_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 31 THEN OutMin ELSE 0 END) AS [31_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [31_STH], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [31_STM]" +
                    ",CAST(SUM(DATEDIFF(second, 0, TotalOverTime)) / 3600 AS varchar(12)) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, TotalOverTime)) " +
                    " / 60 % 60 AS varchar(2)), 2) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, TotalOverTime)) % 60 AS varchar(2)), 2) AS TotalOverTime,DptId,DptName,GId,GName,CompanyName,Address " +
                    "from v_tblAttendanceRecord " +
                    "Where CompanyId " + CompanyId + " " + EmptypeId + "  " + ShiftList + " AND DptId " + DepartmentList + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "'" +
                    "group by EmpId,EmpCardNo,EmpName,DptId,DptName,GId,GName,CompanyName,Address order by empId,DptId ";
                else
                    sql = "select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName, SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN InHour ELSE 0 END) AS [1_InH], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 1 THEN InMin ELSE 0 END) AS [1_InM], SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN OutHour ELSE 0 END) AS [1_OutH], SUM(CASE DATEPART(day, " +
                        "AttDate) WHEN 1 THEN OutMin ELSE 0 END) AS [1_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN CONVERT(int, datepart(hour, TotalOverTime)) " +
                        "ELSE 0 END) AS [1_STH], SUM(CASE DATEPART(day, AttDate) WHEN 1 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [1_STM], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN InHour ELSE 0 END) AS [2_InH], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN InMin ELSE 0 END) " +
                        "AS [2_InM], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN OutHour ELSE 0 END) AS [2_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 2 THEN OutMin ELSE 0 END) AS [2_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        "AS [2_STH], SUM(CASE DATEPART(day, AttDate) WHEN 2 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [2_STM], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN InHour ELSE 0 END) AS [3_InH], SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN InMin ELSE 0 END) " +
                        "AS [3_InM], SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN OutHour ELSE 0 END) AS [3_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 3 THEN OutMin ELSE 0 END) AS [3_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        "AS [3_STH], SUM(CASE DATEPART(day, AttDate) WHEN 3 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [3_STM], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN InHour ELSE 0 END) AS [4_InH], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN InMin ELSE 0 END) " +
                        "AS [4_InM], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN OutHour ELSE 0 END) AS [4_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 4 THEN OutMin ELSE 0 END) AS [4_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        "AS [4_STH], SUM(CASE DATEPART(day, AttDate) WHEN 4 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [4_STM], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 5 THEN InHour ELSE 0 END) AS [5_InH], SUM(CASE DATEPART(day, AttDate) WHEN 5 THEN InMin ELSE 0 END) " +
                        "AS [5_InM], SUM(CASE DATEPART(day, AttDate) WHEN 5 THEN OutHour ELSE 0 END) AS [5_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        "WHEN 5 THEN OutMin ELSE 0 END) AS [5_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 5 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        "AS [5_STH], SUM(CASE DATEPART(day, AttDate) WHEN 5 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [5_STM], " +
                        " SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN InHour ELSE 0 END) AS [6_InH], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN InMin ELSE 0 END) " +
                        "AS [6_InM], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN OutHour ELSE 0 END) AS [6_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        " WHEN 6 THEN OutMin ELSE 0 END) AS [6_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        "AS [6_STH], SUM(CASE DATEPART(day, AttDate) WHEN 6 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [6_STM], " +
                        "SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN InHour ELSE 0 END) AS [7_InH], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN InMin ELSE 0 END) " +
                        "AS [7_InM], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN OutHour ELSE 0 END) AS [7_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        " WHEN 7 THEN OutMin ELSE 0 END) AS [7_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                        " AS [7_STH], SUM(CASE DATEPART(day, AttDate) WHEN 7 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [7_STM], " +
                        " SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN InHour ELSE 0 END) AS [8_InH], SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN InMin ELSE 0 END) " +
                        " AS [8_InM], SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN OutHour ELSE 0 END) AS [8_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        " WHEN 8 THEN OutMin ELSE 0 END) AS [8_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [8_STH], SUM(CASE DATEPART(day, AttDate) WHEN 8 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [8_STM], " +
                         " SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN InHour ELSE 0 END) AS [9_InH], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN InMin ELSE 0 END) " +
                         " AS [9_InM], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN OutHour ELSE 0 END) AS [9_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         " WHEN 9 THEN OutMin ELSE 0 END) AS [9_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         " AS [9_STH], SUM(CASE DATEPART(day, AttDate) WHEN 9 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [9_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 10 THEN InHour ELSE 0 END) AS [10_InH], SUM(CASE DATEPART(day, AttDate) WHEN 10 THEN InMin ELSE 0 END) " +
                         "AS [10_InM], SUM(CASE DATEPART(day, AttDate) WHEN 10 THEN OutHour ELSE 0 END) AS [10_OutH], SUM(CASE DATEPART(day, AttDate) " +
                        " WHEN 10 THEN OutMin ELSE 0 END) AS [10_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 10 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [10_STH], SUM(CASE DATEPART(day, AttDate) WHEN 10 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [10_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN InHour ELSE 0 END) AS [11_InH], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN InMin ELSE 0 END) " +
                         "AS [11_InM], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN OutHour ELSE 0 END) AS [11_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 11 THEN OutMin ELSE 0 END) AS [11_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [11_STH], SUM(CASE DATEPART(day, AttDate) WHEN 11 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [11_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN InHour ELSE 0 END) AS [12_InH], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN InMin ELSE 0 END) " +
                         "AS [12_InM], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN OutHour ELSE 0 END) AS [12_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 12 THEN OutMin ELSE 0 END) AS [12_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [12_STH], SUM(CASE DATEPART(day, AttDate) WHEN 12 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [12_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN InHour ELSE 0 END) AS [13_InH], SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN InMin ELSE 0 END) " +
                         "AS [13_InM], SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN OutHour ELSE 0 END) AS [13_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 13 THEN OutMin ELSE 0 END) AS [13_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [13_STH], SUM(CASE DATEPART(day, AttDate) WHEN 13 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [13_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN InHour ELSE 0 END) AS [14_InH], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN InMin ELSE 0 END) " +
                         "AS [14_InM], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN OutHour ELSE 0 END) AS [14_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 14 THEN OutMin ELSE 0 END) AS [14_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [14_STH], SUM(CASE DATEPART(day, AttDate) WHEN 14 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [14_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 15 THEN InHour ELSE 0 END) AS [15_InH], SUM(CASE DATEPART(day, AttDate) WHEN 15 THEN InMin ELSE 0 END) " +
                         "AS [15_InM], SUM(CASE DATEPART(day, AttDate) WHEN 15 THEN OutHour ELSE 0 END) AS [15_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 15 THEN OutMin ELSE 0 END) AS [15_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 15 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [15_STH], SUM(CASE DATEPART(day, AttDate) WHEN 15 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [15_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN InHour ELSE 0 END) AS [16_InH], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN InMin ELSE 0 END) " +
                         "AS [16_InM], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN OutHour ELSE 0 END) AS [16_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 16 THEN OutMin ELSE 0 END) AS [16_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [16_STH], SUM(CASE DATEPART(day, AttDate) WHEN 16 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [16_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN InHour ELSE 0 END) AS [17_InH], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN InMin ELSE 0 END) " +
                         "AS [17_InM], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN OutHour ELSE 0 END) AS [17_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 17 THEN OutMin ELSE 0 END) AS [17_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [17_STH], SUM(CASE DATEPART(day, AttDate) WHEN 17 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [17_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN InHour ELSE 0 END) AS [18_InH], SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN InMin ELSE 0 END) " +
                         "AS [18_InM], SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN OutHour ELSE 0 END) AS [18_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 18 THEN OutMin ELSE 0 END) AS [18_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [18_STH], SUM(CASE DATEPART(day, AttDate) WHEN 18 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [18_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN InHour ELSE 0 END) AS [19_InH], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN InMin ELSE 0 END) " +
                         "AS [19_InM], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN OutHour ELSE 0 END) AS [19_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 19 THEN OutMin ELSE 0 END) AS [19_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [19_STH], SUM(CASE DATEPART(day, AttDate) WHEN 19 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [19_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 20 THEN InHour ELSE 0 END) AS [20_InH], SUM(CASE DATEPART(day, AttDate) WHEN 20 THEN InMin ELSE 0 END) " +
                         "AS [20_InM], SUM(CASE DATEPART(day, AttDate) WHEN 20 THEN OutHour ELSE 0 END) AS [20_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 20 THEN OutMin ELSE 0 END) AS [20_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 20 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [20_STH], SUM(CASE DATEPART(day, AttDate) WHEN 20 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [20_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN InHour ELSE 0 END) AS [21_InH], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN InMin ELSE 0 END) " +
                         "AS [21_InM], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN OutHour ELSE 0 END) AS [21_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 21 THEN OutMin ELSE 0 END) AS [21_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [21_STH], SUM(CASE DATEPART(day, AttDate) WHEN 21 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [21_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN InHour ELSE 0 END) AS [22_InH], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN InMin ELSE 0 END) " +
                         "AS [22_InM], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN OutHour ELSE 0 END) AS [22_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 22 THEN OutMin ELSE 0 END) AS [22_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [22_STH], SUM(CASE DATEPART(day, AttDate) WHEN 22 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [22_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN InHour ELSE 0 END) AS [23_InH], SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN InMin ELSE 0 END) " +
                         "AS [23_InM], SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN OutHour ELSE 0 END) AS [23_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 23 THEN OutMin ELSE 0 END) AS [23_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [23_STH], SUM(CASE DATEPART(day, AttDate) WHEN 23 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [23_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN InHour ELSE 0 END) AS [24_InH], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN InMin ELSE 0 END) " +
                         "AS [24_InM], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN OutHour ELSE 0 END) AS [24_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 24 THEN OutMin ELSE 0 END) AS [24_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [24_STH], SUM(CASE DATEPART(day, AttDate) WHEN 24 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [24_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 25 THEN InHour ELSE 0 END) AS [25_InH], SUM(CASE DATEPART(day, AttDate) WHEN 25 THEN InMin ELSE 0 END) " +
                         "AS [25_InM], SUM(CASE DATEPART(day, AttDate) WHEN 25 THEN OutHour ELSE 0 END) AS [25_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 25 THEN OutMin ELSE 0 END) AS [25_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 25 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [25_STH], SUM(CASE DATEPART(day, AttDate) WHEN 25 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [25_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN InHour ELSE 0 END) AS [26_InH], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN InMin ELSE 0 END) " +
                         "AS [26_InM], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN OutHour ELSE 0 END) AS [26_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 26 THEN OutMin ELSE 0 END) AS [26_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [26_STH], SUM(CASE DATEPART(day, AttDate) WHEN 26 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [26_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN InHour ELSE 0 END) AS [27_InH], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN InMin ELSE 0 END) " +
                         "AS [27_InM], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN OutHour ELSE 0 END) AS [27_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 27 THEN OutMin ELSE 0 END) AS [27_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [27_STH], SUM(CASE DATEPART(day, AttDate) WHEN 27 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [27_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN InHour ELSE 0 END) AS [28_InH], SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN InMin ELSE 0 END) " +
                         "AS [28_InM], SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN OutHour ELSE 0 END) AS [28_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 28 THEN OutMin ELSE 0 END) AS [28_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [28_STH], SUM(CASE DATEPART(day, AttDate) WHEN 28 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [28_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN InHour ELSE 0 END) AS [29_InH], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN InMin ELSE 0 END) " +
                         "AS [29_InM], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN OutHour ELSE 0 END) AS [29_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 29 THEN OutMin ELSE 0 END) AS [29_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [29_STH], SUM(CASE DATEPART(day, AttDate) WHEN 29 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [29_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 30 THEN InHour ELSE 0 END) AS [30_InH], SUM(CASE DATEPART(day, AttDate) WHEN 30 THEN InMin ELSE 0 END) " +
                         "AS [30_InM], SUM(CASE DATEPART(day, AttDate) WHEN 30 THEN OutHour ELSE 0 END) AS [30_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 30 THEN OutMin ELSE 0 END) AS [30_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 30 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [30_STH], SUM(CASE DATEPART(day, AttDate) WHEN 30 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [30_STM], " +
                         "SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN InHour ELSE 0 END) AS [31_InH], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN InMin ELSE 0 END) " +
                         "AS [31_InM], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN OutHour ELSE 0 END) AS [31_OutH], SUM(CASE DATEPART(day, AttDate) " +
                         "WHEN 31 THEN OutMin ELSE 0 END) AS [31_OutM], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN CONVERT(int, datepart(hour, TotalOverTime)) ELSE 0 END) " +
                         "AS [31_STH], SUM(CASE DATEPART(day, AttDate) WHEN 31 THEN CONVERT(int, datepart(MINUTE, TotalOverTime)) ELSE 0 END) AS [31_STM]" +
                    ",CAST(SUM(DATEDIFF(second, 0, TotalOverTime)) / 3600 AS varchar(12)) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, TotalOverTime)) " +
                    " / 60 % 60 AS varchar(2)), 2) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, TotalOverTime)) % 60 AS varchar(2)), 2) AS TotalOverTime,DptId,DptName,GId,GName,CompanyName,Address " +
                "from v_tblAttendanceRecord " +
                "Where CompanyId " + CompanyId + " " + EmptypeId + " AND MONTH(ATTDate) ='" + Month + "' AND Year(ATTDate)='" + Year + "' AND Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + EmpCardNo + "'" +
                "group by EmpId,EmpCardNo,EmpName,DptId,DptName,GId,GName,CompanyName,Address ";

                sqlDB.fillDataTable(sql, dt = new DataTable());
                return dt;
            }
            catch { return null; }
        }
        //-----------------------------------------------------------------------------
    }
}