#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Cake.Addin.Extensions;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Cake.Addin.CodeSigning
{
	public static partial class Aliases
	{
		[global::Cake.Core.Annotations.CakeMethodAlias]
		public static GetSignAssemblyCommandResponse GetSignAssemblyCommand(this global::Cake.Core.ICakeContext cakeContext, IGetSignAssemblyCommandRequest request)
		{
			ServiceProvider.Initialize();

			var response = new GetSignAssemblyCommandResponse();

			var signAssembliesRequest = request as GetSignAssemblyCommandRequest;

			if (request is GetSignAssemblyCommandUsingSettingsRequest signAssembliesUsingSettingsRequest)
			{
				signAssembliesRequest = new GetSignAssemblyCommandRequest()
				{
					RemoteCodeSigningServiceUri = cakeContext.GetNullableUri(signAssembliesUsingSettingsRequest.Settings.CodeSigning.RemoteCodeSigningServiceApiUrl),
					RemoteCodeSigningServiceApiKey = signAssembliesUsingSettingsRequest.Settings.CodeSigning.RemoteCodeSigningServiceApiKey,
					CodeSigningCertificateTokenCertificateFileName = signAssembliesUsingSettingsRequest.Settings.CodeSigning.Token.CertificateFileName,
					CodeSigningCertificateTokenCryptographicProvider = signAssembliesUsingSettingsRequest.Settings.CodeSigning.Token.CryptographicProvider,
					CodeSigningCertificateTokenContainerName = signAssembliesUsingSettingsRequest.Settings.CodeSigning.Token.ContainerName,
					CodeSigningCertificateTokenPassword = signAssembliesUsingSettingsRequest.Settings.CodeSigning.Token.Password,
					TimeStampUri = cakeContext.GetNullableUri(signAssembliesUsingSettingsRequest.Settings.CodeSigning.TimeStampUrl),
					TimeStampDigestAlgorithm = cakeContext.GetCodeSigningDigestAlgorithm(signAssembliesUsingSettingsRequest.Settings.CodeSigning.TimeStampDigestAlgorithm),
					CertificatePath = cakeContext.GetNullableFile(signAssembliesUsingSettingsRequest.Settings.CodeSigning.CertificateFileName),
					CertificatePassword = signAssembliesUsingSettingsRequest.Settings.CodeSigning.CertificatePassword,
					CertificateFingerprint = signAssembliesUsingSettingsRequest.Settings.CodeSigning.CertificateFingerprint,
					DigestAlgorithm = cakeContext.GetCodeSigningDigestAlgorithm(signAssembliesUsingSettingsRequest.Settings.CodeSigning.DigestAlgorithm),
					RunAsync = signAssembliesUsingSettingsRequest.Settings.CodeSigning.RunAsync,
					UseShortPathName = request.UseShortPathName,
				};
			}

			if (signAssembliesRequest.RemoteCodeSigningServiceUri == null)
			{
				var logger = new CakeContextLogger(cakeContext);

				var jsonSerializer = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.JsonSerialization.IJsonSerializer>();

				var codeSigningApi = new ISI.Extensions.VisualStudio.CodeSigningApi(logger, new ISI.Extensions.Nuget.NugetApi(ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Nuget.Configuration>(), logger, jsonSerializer));

				var getSignAssemblyCommandResponse = codeSigningApi.GetSignAssemblyCommand(new ()
				{
					CodeSigningCertificateTokenCertificateFileName = signAssembliesRequest.CodeSigningCertificateTokenCertificateFileName,
					CodeSigningCertificateTokenCryptographicProvider = signAssembliesRequest.CodeSigningCertificateTokenCryptographicProvider,
					CodeSigningCertificateTokenContainerName = signAssembliesRequest.CodeSigningCertificateTokenContainerName,
					CodeSigningCertificateTokenPassword = signAssembliesRequest.CodeSigningCertificateTokenPassword,
					TimeStampUri = signAssembliesRequest.TimeStampUri,
					TimeStampDigestAlgorithm = signAssembliesRequest.TimeStampDigestAlgorithm.ToCodeSigningDigestAlgorithm(),
					CertificateFileName = signAssembliesRequest.CertificatePath?.FullPath,
					CertificatePassword = signAssembliesRequest.CertificatePassword,
					CertificateSubjectName = signAssembliesRequest.CertificateSubjectName,
					CertificateFingerprint = signAssembliesRequest.CertificateFingerprint,
					DigestAlgorithm = signAssembliesRequest.DigestAlgorithm.ToCodeSigningDigestAlgorithm(),
					OverwriteAnyExistingSignature = signAssembliesRequest.OverwriteAnyExistingSignature,
					Verbosity = signAssembliesRequest.Verbosity.ToCodeSigningVerbosity(),
					UseShortPathName = request.UseShortPathName,
				});

				response.Command = getSignAssemblyCommandResponse.Command;
			}

			return response;
		}
	}
}