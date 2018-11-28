﻿// <autogenerated>
//   This file was generated by T4 code generator Generate.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

namespace Wageworks.Feature.Navigation.Models.Glass
{
	using System;
    using System.Collections.Generic;
	using System.Collections.Specialized;
    using global::Glass.Mapper.Sc.Configuration;
    using global::Glass.Mapper.Sc.Configuration.Attributes;
	using global::Glass.Mapper.Sc.Fields;
    using Navigation;


	/// <summary>
	/// Represents a mapped type for item {18BAF6B0-E0D6-4CCE-9184-A4849343E7E4} in Sitecore.
	/// Path: /sitecore/templates/Feature/Navigation/_LinkMenuItem
	/// </summary>
	[SitecoreType(TemplateId = "{18BAF6B0-E0D6-4CCE-9184-A4849343E7E4}")]
	public partial interface ILinkMenuItem : ILink, ILinkDescription
	{
		#region Navigation

		/// <summary>
		/// Divider Before
		/// </summary>
	    [SitecoreField(FieldId = "{4231CD60-47C1-42AD-B838-0A6F8F1C4CFB}")]
		bool DividerBefore { get; set; }

		/// <summary>
		/// Icon or Image
		/// </summary>
	    [SitecoreField(FieldId = "{2C24649E-4460-4114-B026-886CFBE1A96D}")]
		Image Icon { get; set; }

		#endregion

	}
}
