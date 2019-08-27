using System.IO;

namespace Al.Security.Asn1
{
	public interface Asn1OctetStringParser
		: IAsn1Convertible
	{
		Stream GetOctetStream();
	}
}
