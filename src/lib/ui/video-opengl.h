#ifndef __AVBOX_VIDEO_OPENGL__
#define __AVBOX_VIDEO_OPENGL__

#include "video-drv.h"

/**
 * Initialize the opengl driver
 */
struct mbv_surface *
avbox_video_glinit(
	struct mbv_drv_funcs * const funcs,
	int width, const int height,
	void (*swap_buffers_fn)(void));

#endif
