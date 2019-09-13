using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilders.WebControls
{
	internal interface ISelectorFieldControl
	{
		Boolean Selected
		{
			get;
			set;
		}

		Int32 Index
		{
			get;
			set;
		}

		Boolean AutoPostBack
		{
			get;
			set;
		}

	}
}
