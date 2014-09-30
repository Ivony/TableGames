using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public class JsonContentNegotiator : DefaultContentNegotiator
  {

    protected override MediaTypeFormatterMatch MatchAcceptHeader( IEnumerable<System.Net.Http.Headers.MediaTypeWithQualityHeaderValue> sortedAcceptValues, MediaTypeFormatter formatter )
    {

      return null;

      return base.MatchAcceptHeader( sortedAcceptValues, formatter );
    }


    protected override MediaTypeFormatterMatch MatchMediaTypeMapping( System.Net.Http.HttpRequestMessage request, MediaTypeFormatter formatter )
    {
      return base.MatchMediaTypeMapping( request, formatter );
    }

    protected override MediaTypeFormatterMatch MatchRequestMediaType( System.Net.Http.HttpRequestMessage request, MediaTypeFormatter formatter )
    {
      return base.MatchRequestMediaType( request, formatter );
    }

    protected override System.Collections.ObjectModel.Collection<MediaTypeFormatterMatch> ComputeFormatterMatches( Type type, System.Net.Http.HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters )
    {
      return base.ComputeFormatterMatches( type, request, formatters );
    }

    protected override MediaTypeFormatterMatch SelectResponseMediaTypeFormatter( ICollection<MediaTypeFormatterMatch> matches )
    {
      return base.SelectResponseMediaTypeFormatter( matches );
    }
  }
}