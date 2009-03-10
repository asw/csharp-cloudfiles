///
/// See COPYING file for licensing information
///

using System;

namespace com.mosso.cloudfiles.domain.request
{
    public class SetPublicContainerDetails : BaseRequest
    {
        /// <summary>
        /// Assigns various details to containers already publicly available on the CDN
        /// </summary>
        /// <param name="cdnManagementUrl">The CDN URL</param>
        /// <param name="authToken">The authorization token returned from the authorization server</param>
        /// <param name="containerName">The name of the container to update the details for</param>
        /// <param name="isCdnEnabled">Sets whether or not specified container is available on the CDN</param>
        public SetPublicContainerDetails(string cdnManagementUrl, string authToken, string containerName, bool isCdnEnabled)
        {
            if (String.IsNullOrEmpty(cdnManagementUrl) ||
                    String.IsNullOrEmpty(authToken) ||
                    String.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            Method = "POST";

            Uri = new Uri(cdnManagementUrl + "/" + containerName.Encode());

            AddStorageOrAuthTokenToHeaders(Constants.X_AUTH_TOKEN, authToken);
            Headers.Add(Constants.X_CDN_ENABLED, isCdnEnabled.Capitalize());
        }
    }
}