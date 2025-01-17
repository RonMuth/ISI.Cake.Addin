#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;

namespace ISI.Cake.Addin.Docker
{
	public class SetDockerImageDetailsRequest
	{
		public string Project { get; set; }

		public string TargetOperatingSystem { get; set; }
		public string ContainerRegistry { get; set; }
		public string ContainerRepository { get; set; }
		public string[] ContainerImageTags { get; set; }
		public string ContainerFamily { get; set; }
		public string ContainerBaseImage { get; set; }
		public string ContainerWorkingDirectory { get; set; }

		public string GetRuntime(global::Cake.Common.Tools.MSBuild.MSBuildPlatform msBuildPlatform)
		{
			if (msBuildPlatform == global::Cake.Common.Tools.MSBuild.MSBuildPlatform.Automatic)
			{
				return $"{TargetOperatingSystem}-x64".ToLower();
			}

			return $"{TargetOperatingSystem}-{msBuildPlatform.GetKey()}".ToLower();
		}

		public string GetRuntimes(global::Cake.Common.Tools.MSBuild.MSBuildPlatform msBuildPlatform)
		{
			switch (msBuildPlatform)
			{
				case global::Cake.Common.Tools.MSBuild.MSBuildPlatform.Automatic:
				case global::Cake.Common.Tools.MSBuild.MSBuildPlatform.x64:
					return $"{TargetOperatingSystem}-x64,{TargetOperatingSystem}-arm64".ToLower();

				case global::Cake.Common.Tools.MSBuild.MSBuildPlatform.x86:
					return $"{TargetOperatingSystem}-x86,{TargetOperatingSystem}-arm".ToLower();

				default:
					throw new ArgumentOutOfRangeException(nameof(msBuildPlatform), msBuildPlatform, null);
			}
		}
	}
}