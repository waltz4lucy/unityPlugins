#import <CoreTelephony/CTCarrier.h>
#import <CoreTelephony/CTTelephonyNetworkInfo.h>

extern "C"
{
    char* _MakeStringCopy(const char* string)
    {
        if (string == NULL)
            return NULL;

        char* res = (char*)malloc(strlen(string) + 1);
        strcpy(res, string);

        return res;
    }

	const char* IOSNativeGetMcc()
	{
		CTTelephonyNetworkInfo* networkInfo = [[CTTelephonyNetworkInfo alloc] init];
		CTCarrier* carrier = [networkInfo subscriberCellularProvider];

		if (carrier == nil || carrier.mobileCountryCode == nil)
            return _MakeStringCopy("");

		return _MakeStringCopy([[carrier mobileCountryCode] UTF8String]);
	}
}
