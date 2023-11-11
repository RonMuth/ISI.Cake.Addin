using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Cake.Addin.Docker
{
	public class GetDockerImageDetailsResponse
	{
		public string TargetOperatingSystem { get; set; }
		public string ContainerImageName { get; set; }
		public string ContainerImageTag { get; set; }
	}
}