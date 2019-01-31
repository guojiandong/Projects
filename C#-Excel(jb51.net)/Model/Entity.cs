using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 用户实体类 tb_quality
    /// </summary>
	public class QualityEntity
	{
        const int DefualtErrorFlag = 1;
        /// <summary>
        /// .Ctor
        /// </summary>
        public QualityEntity()
        {
        }

        /// <summary>
        /// 唯一标识 Key
        /// </summary>
        public int Id { get; set; } = DefualtErrorFlag;

        /// <summary>
        /// 段位  M1 - M2 - M3 - TP - Offline
        /// </summary>
        public string Section { get; set; } = string.Empty;

        /// <summary>
        /// 问题描述 eg：定位柱损伤
        /// </summary>
        public string ProblemName { get; set; } = string.Empty;

        /// <summary>
        /// 产线Code
        /// </summary>
        public int ProcessId { get; set; } = DefualtErrorFlag;


        /// <summary>
        /// 产线LineId
        /// </summary>
        public int LineId { get; set; } = DefualtErrorFlag;

        /// <summary>
        /// 门类Id
        /// </summary>
        public int SubjectId { get; set; } = DefualtErrorFlag;

        /// <summary>
        /// 类型Id
        /// </summary>
        public int ClassId { get; set; } = DefualtErrorFlag;

        /// <summary>
        /// 日期 2018-11-07
        /// </summary>
        public string Date { get; set; } = string.Empty;

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; } = DefualtErrorFlag;

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; } = DefualtErrorFlag;

        /// <summary>
        /// 天
        /// </summary>
        public int Day { get; set; } = DefualtErrorFlag;

        /// <summary>
        /// 周
        /// </summary>
        public int Week { get; set; } = DefualtErrorFlag;

        /// <summary>
        /// 产线code
        /// </summary>
        public string UserLineCode { get; set; } = string.Empty;

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; } = DefualtErrorFlag;

        /// <summary>
        /// 最好
        /// </summary>
        public int Best { get; set; } = 0;

        /// <summary>
        /// 普通
        /// </summary>
        public int Norm { get; set; } = 0;

        /// <summary>
        /// 最好的
        /// </summary>
        public double TheBest { get; set; } = -1.0;

        /// <summary>
        /// 普通的
        /// </summary>
        public double TheNorm { get; set; } = -1.0;
	}

    /// <summary>
    /// tb_userline
    /// </summary>
    public class UserLineEntity
    {
        const int DefualtErrorFlag = -1;

        /// <summary>
        /// .Ctor
        /// </summary>
        public UserLineEntity()
        {
        }
        /// <summary>
        /// .Ctor
        /// </summary>
        /// 
        public UserLineEntity(int id, string code, string name, int lineid,int index, int del)
        {
            Id = id;
            Code = code;
            Name = name;
            LineId = lineid;
            Index = index;
            Del = del;
        }

        /// <summary>
        /// 唯一标识 Key
        /// </summary>
        public int Id { get; set; } = DefualtErrorFlag;

        /// <summary>
        /// 产线code
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 产线名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public int LineId { get; set; } 

        /// <summary>
        /// 产线Id
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public int Del { get; set; }
    }

    /// <summary>
    /// tb_subject 
    /// </summary>
    public class SubjectEntity
    {
        const int DefualtErrorFlag = -1;
        /// <summary>
        /// .Ctor
        /// </summary>
        public SubjectEntity()
        {
        }

        /// <summary>
        /// .Ctor
        /// </summary>
        public SubjectEntity(int id, string name, int processid)
        {
            Id = id;
            Name = name;
            ProcessId = processid;
        }

        /// <summary>
        /// 唯一标识 Key
        /// </summary>
        public int Id { get; set; } = DefualtErrorFlag;

        /// <summary>
        /// 产线名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ProcessId
        /// </summary>
        public int ProcessId { get; set; }
    }


    public class ProcessEntity
    {
        public ProcessEntity()
        {
        }

        public ProcessEntity(int id, string code ,int thebest, int thenorm)
        {
            Id = id;
            Code = code;
            TheBest = thebest;
            TheNorm = thenorm;
        }

        public int Id { get; set; }

        public string Code { get; set; }

        public int TheBest { get; set; }

        public int TheNorm { get; set; }
    }

}
