using System;

namespace Al.Security.Cms
{
	internal interface IDigestCalculator
	{
		byte[] GetDigest();
	}
}
