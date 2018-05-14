using System;
using System.ComponentModel.DataAnnotations;

namespace CardinalWebApplication.Models.DbContext
{
    public class ApplicationOption
    {
        [Key]
        public int OptionsId { get; set; }
        public DateTime OptionsDate { get; set; }
        public string EndUserLicenseAgreementSource { get; set; }
        public string TermsConditionsSource { get; set; }
        public string PrivacyPolicySource { get; set; }
        public TimeSpan DataTimeWindow { get; set; }
        public int Version { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
    }
}
