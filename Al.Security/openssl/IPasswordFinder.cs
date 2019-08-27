using System;

namespace Al.Security.OpenSsl
{
	public interface IPasswordFinder
	{
		char[] GetPassword();
	}
}
