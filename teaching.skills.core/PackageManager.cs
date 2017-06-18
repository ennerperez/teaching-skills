using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Teaching.Skills.Models;

namespace Teaching.Skills
{
	public class PackageManager
	{

		#region Singleton

		private static readonly PackageManager instance = new PackageManager();

		private PackageManager()
		{
		}

		public static PackageManager Instance
		{
			get
			{
				return instance;
			}
		}

		#endregion Singleton

		public const string Repo = "http://192.168.0.100/packages/";

		public IList<Package> Packages { get; private set; }

		public async Task RequestPackagesAsync()
		{

			var request = WebRequest.Create(Repo + "index.json");
			var response = await request.GetResponseAsync();

			using (Stream stream = response.GetResponseStream())
			{
				var reader = new StreamReader(stream, Encoding.UTF8);
				Packages = new List<Package>(JsonConvert.DeserializeObject<IEnumerable<Package>>(reader.ReadToEnd()));
			}

		}

		public async Task<Package> DownloadPackageAsync(string key)
		{

			Package result = null;
			var package = Packages.FirstOrDefault((arg) => arg.Key() == key);
			if (package != null)
			{
				var request = WebRequest.Create(package.Url);
				var response = await request.GetResponseAsync();

				using (Stream stream = response.GetResponseStream())
				{
					var reader = new StreamReader(stream, Encoding.UTF8);
					result = JsonConvert.DeserializeObject<Package>(reader.ReadToEnd());
				}
			}
			return result;
		}

	}
}

