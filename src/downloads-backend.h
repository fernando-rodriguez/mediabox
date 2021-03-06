/**
 * MediaBox - Linux based set-top firmware
 * Copyright (C) 2016-2017 Fernando Rodriguez
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License Version 3 as 
 * published by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */


#ifndef __MB_DLBE_H__
#define __MB_DLBE_H__

struct mbox_dlman_download_item
{
	int type;
	int percent;
	void *stream;
	const char *id;
	const char *name;
};



int
mbox_dlman_addurl(const char * const url);


struct mbox_dlman_download_item*
mbox_dlman_next(struct mbox_dlman_download_item * const current);


void
mbox_dlman_item_unref(struct mbox_dlman_download_item * const inst);


int
mb_downloadmanager_init(void);


void
mb_downloadmanager_destroy(void);

#endif
