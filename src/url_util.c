#ifdef HAVE_CONFIG_H
#include "config.h"
#endif
#include <stdlib.h>
#include <string.h>
#include <curl/curl.h>

struct MemoryStruct
{
	char *memory;
	size_t size;
	size_t limit;
};


static size_t
WriteMemoryCallback(void *contents, size_t size, size_t nmemb, void *userp)
{
	size_t realsize = size * nmemb;
	struct MemoryStruct *mem = (struct MemoryStruct *)userp;

	mem->memory = realloc(mem->memory, mem->size + realsize + 1);
	if(mem->memory == NULL) {
		/* out of memory! */
		fprintf(stderr, "not enough memory (realloc returned NULL)\n");
		return 0;
	}

	memcpy(&(mem->memory[mem->size]), contents, realsize);
	mem->size += realsize;
	mem->memory[mem->size] = 0;

	return realsize;
}


int
mb_url_fetch2mem(char *url, void **dest, size_t *size)
{
	CURL *curl_handle;
	CURLcode res;
	int ret = -1;

	struct MemoryStruct chunk;

	chunk.memory = malloc(1);
	chunk.size = 0;
	if (chunk.memory == NULL) {
		return -1;
	}

	curl_global_init(CURL_GLOBAL_ALL);

	/* init the curl session */
	if ((curl_handle = curl_easy_init()) == NULL) {
		fprintf(stderr, "url_util: curl_easy_init() failed\n");
		goto end;
	}

	curl_easy_setopt(curl_handle, CURLOPT_URL, url);
	curl_easy_setopt(curl_handle, CURLOPT_WRITEFUNCTION, WriteMemoryCallback);
	curl_easy_setopt(curl_handle, CURLOPT_WRITEDATA, (void *)&chunk);
	curl_easy_setopt(curl_handle, CURLOPT_ACCEPT_ENCODING, "");
	curl_easy_setopt(curl_handle, CURLOPT_USERAGENT, "mediabox/" PACKAGE_VERSION);
	//curl_easy_setopt(curl_handle, CURLOPT_USERAGENT, "Mozilla/5.0 (X11; Linux x86_64; rv:46.0) Gecko/20100101 Firefox/46.0");

	/* get it! */
	res = curl_easy_perform(curl_handle);

	/* check for errors */
	if(res != CURLE_OK) {
		fprintf(stderr, "curl_easy_perform() failed: %s\n",
			curl_easy_strerror(res));
		goto end;

	} else {
		*dest = chunk.memory;
		*size = chunk.size;
		ret = 0;
	}

end:

	if (ret == -1) {
		free(chunk.memory);
	}

	curl_easy_cleanup(curl_handle);
	curl_global_cleanup();
	return ret;
}

