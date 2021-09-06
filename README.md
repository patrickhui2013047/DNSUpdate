# DNSUpdate
An auto IP address updater for user that using Cloudflare DNS service with a dynamic IP address.

The program will auto capture the public IP address on the running maechine and update the dns record on Cloundflare.

## Install
Unzip the release zip file.

## Usage
Run `DNSUpdate.exe` to execute the program first. a config file name `dnsUpdater.conf` will create. Put the setting as following.
````json
{
	"ZoneID":"Your zone ID",
	"DnsID":"Your Dns ID",
	"AuthKey":"Your AuthKey",
	"AuthEMail":"Your AuthEmail",
	"PreferedDomain":"Your prefered domain to update"
}
```
