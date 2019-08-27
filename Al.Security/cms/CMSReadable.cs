using System;
using System.IO;

namespace Al.Security.Cms
{
	internal interface CmsReadable
	{
		Stream GetInputStream();
	}
}
