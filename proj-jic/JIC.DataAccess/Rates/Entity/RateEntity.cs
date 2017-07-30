using System;

namespace JIC.DataAccess.Rates.Entity
{
    public class RateEntity
    {
        public string PRCF_PROD { get; set; }
        public int PRCF_DATE { get; set; }
        public string PRCF_COVER_P { get; set; }
        public string PRCF_COVER_S { get; set; }
        public float PRCF_AGE { get; set; }
        public decimal PRCF_DUR { get; set; }
        public string PRCF_SEX { get; set; }
        public string PRCF_SMK { get; set; }
        public string PRCF_PAY { get; set; }
        public decimal PRCF_PERI { get; set; }
        public string PRCF_CLS { get; set; }
        public float PRCF_RAT_MIN { get; set; }
        public float PRCF_RAT_MAX { get; set; }
        public float PRCF_AMNT_MIN { get; set; }
        public float PRCF_AMNT_MAX { get; set; }
        public string PRCF_USER_IDC { get; set; }
        public string PRCF_USER_IDM { get; set; }
        public DateTime PRCF_DATE_STAMPC { get; set; }
        public DateTime PRCF_DATE_STAMPM { get; set; }
        public float PRCF_BEN_AGE { get; set; }

    }
}
