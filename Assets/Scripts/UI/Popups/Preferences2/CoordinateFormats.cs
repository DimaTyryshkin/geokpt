using System;
using System.Collections.Generic;
using System.Linq;
using Geo.Data;
using Geo.KptData.Converters;
using UnityEngine.Assertions;

namespace Geo.UI
{
	public abstract class FormatPart
	{
		protected abstract SeparatorInfo[] Default { get; }
		protected abstract List<string>    UserValues    { get; }

		public abstract string Value { get; set; }
		public abstract bool   CanAddUserValues { get; }
		public abstract string Header { get; }

		public string[] GetAvailableValues()
		{
			return Default
				.Select(s => s.separator)
				.Concat(UserValues)
				.ToArray();
		}

		public string GetCurrentLabel()
		{
			return GetLabel(Value);
		}

		public string GetLabel(string value)
		{
			foreach (SeparatorInfo info in Default)
			{
				if (value == info.separator)
					return info.label;
			}

			return $"'{value}'";
		}

		public bool AddUserValue(string value)
		{
			if (!CanAddUserValues)
				throw new NotSupportedException();

			if (GetAvailableValues().Contains(value))
				return false;

			UserValues.Add(value);
			return true;
		}
	}

	public class DecimalSeparatorFormatPart : FormatPart
	{
		AccountData.ContourToTxtConverterPreferences2 preferences2;

		protected override SeparatorInfo[] Default => DecimalSeparatorsList.decimals;
		protected override List<string>    UserValues    => new List<string>();

		public override string Value
		{
			get => preferences2.decimalSeparator;
			set => preferences2.decimalSeparator = value;
		}

		public override bool   CanAddUserValues => false;
		public override string Header           => "Десятичный разделитель";

		public DecimalSeparatorFormatPart(AccountData.ContourToTxtConverterPreferences2 preferences2)
		{
			Assert.IsNotNull(preferences2);
			this.preferences2 = preferences2;
		}
	}

	public class SeparatorFormatPart : FormatPart
	{
		AccountData.ContourToTxtConverterPreferences2 preferences2;
		CoordinateFormats2Config                      config;

		protected override SeparatorInfo[] Default => config.defaultSeparators;
		protected override List<string>    UserValues    => preferences2.userSeparators;

		public override string Value
		{
			get => preferences2.separator;
			set => preferences2.separator = value;
		}

		public override bool   CanAddUserValues => true;
		public override string Header           => "Разделитель";

		public SeparatorFormatPart(AccountData.ContourToTxtConverterPreferences2 preferences2, CoordinateFormats2Config config)
		{
			Assert.IsNotNull(preferences2);
			Assert.IsNotNull(config);
			this.preferences2 = preferences2;
			this.config       = config;
		}
	}

	public class PointIndexFormatsFormatPart : FormatPart
	{
		AccountData.ContourToTxtConverterPreferences2 preferences2;
		CoordinateFormats2Config                      config;

		protected override SeparatorInfo[] Default => config.defaultPointIndexFormats;
		protected override List<string>    UserValues    => new List<string>();

		public override string Value
		{
			get => preferences2.pointIndexFormat;
			set => preferences2.pointIndexFormat = value;
		}

		public override bool   CanAddUserValues => false;
		public override string Header           => "Номер точки";

		public PointIndexFormatsFormatPart(AccountData.ContourToTxtConverterPreferences2 preferences2, CoordinateFormats2Config config)
		{
			Assert.IsNotNull(preferences2);
			Assert.IsNotNull(config);
			this.preferences2 = preferences2;
			this.config       = config;
		}
	}

	public class HeightFormatPart : FormatPart
	{
		AccountData.ContourToTxtConverterPreferences2 preferences2;
		CoordinateFormats2Config                      config;

		protected override SeparatorInfo[] Default => config.defaultHeight;
		protected override List<string>    UserValues    => new List<string>();

		public override string Value
		{
			get => preferences2.height;
			set => preferences2.height = value;
		}

		public override bool   CanAddUserValues => false;
		public override string Header           => "Высота точки";

		public HeightFormatPart(AccountData.ContourToTxtConverterPreferences2 preferences2, CoordinateFormats2Config config)
		{
			Assert.IsNotNull(preferences2);
			Assert.IsNotNull(config);
			this.preferences2 = preferences2;
			this.config       = config;
		}
	}

	public class CoordinateFormats
	{
		readonly CoordinateFormats2Config                      config;
		readonly AccountData.ContourToTxtConverterPreferences2 preferences2;

		public readonly DecimalSeparatorFormatPart  DecimalSeparatorFormatPart;
		public readonly SeparatorFormatPart         SeparatorFormatPart;
		public readonly PointIndexFormatsFormatPart PointIndexFormatsFormatPart;
		public readonly HeightFormatPart            HeightFormatPart;

		public CoordinateFormats(CoordinateFormats2Config config, AccountData.ContourToTxtConverterPreferences2 preferences2)
		{
			Assert.IsNotNull(config);
			Assert.IsNotNull(preferences2);
			this.config       = config;
			this.preferences2 = preferences2;

			DecimalSeparatorFormatPart  = new DecimalSeparatorFormatPart(preferences2);
			SeparatorFormatPart         = new SeparatorFormatPart(preferences2, config);
			PointIndexFormatsFormatPart = new PointIndexFormatsFormatPart(preferences2, config);
			HeightFormatPart            = new HeightFormatPart(preferences2, config);
		}
	}
}