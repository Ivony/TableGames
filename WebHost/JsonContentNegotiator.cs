using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public class JsonContentNegotiator : DefaultContentNegotiator
  {

    protected override MediaTypeFormatterMatch MatchAcceptHeader( IEnumerable<MediaTypeWithQualityHeaderValue> sortedAcceptValues, MediaTypeFormatter formatter )
    {

      return base.MatchAcceptHeader( sortedAcceptValues, formatter );
    }


    protected override MediaTypeFormatterMatch MatchMediaTypeMapping( HttpRequestMessage request, MediaTypeFormatter formatter )
    {
      return base.MatchMediaTypeMapping( request, formatter );
    }

    protected override MediaTypeFormatterMatch MatchRequestMediaType( HttpRequestMessage request, MediaTypeFormatter formatter )
    {
      return base.MatchRequestMediaType( request, formatter );
    }

    protected override Collection<MediaTypeFormatterMatch> ComputeFormatterMatches( Type type, System.Net.Http.HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters )
    {
      return base.ComputeFormatterMatches( type, request, formatters );
    }

    protected override MediaTypeFormatterMatch SelectResponseMediaTypeFormatter( ICollection<MediaTypeFormatterMatch> matches )
    {

      var jsonMatch = matches.FirstOrDefault( item => item.MediaType.MediaType == "application/json" );
      if ( jsonMatch != null )
        return jsonMatch;


      return base.SelectResponseMediaTypeFormatter( matches );
    }
  }
}