﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.IIS.Administration.WebServer.Files
{
    using Core;
    using System;

    public class FileId
    {
        private const string PURPOSE = "WebServer.Files";
        private const char DELIMITER = '\n';
        private const uint PATH_INDEX = 0;
        private const uint SITE_ID_NUM_INDEX = 1;

        private string _path;

        public FileId(string uuid)
        {
            if (String.IsNullOrEmpty(uuid)) {
                throw new ArgumentNullException("uuid");
            }
            string[] info = Core.Utils.Uuid.Decode(uuid, PURPOSE).Split(DELIMITER);

            string path = info[PATH_INDEX];
            string siteId = info[SITE_ID_NUM_INDEX];

            this.SiteId = long.Parse(info[SITE_ID_NUM_INDEX]);
            this.Path = info[PATH_INDEX];
            this.Uuid = uuid;
        }

        public FileId(long siteId, string path)
        {
            this.Path = path;
            this.SiteId = siteId;

            this.Uuid = Core.Utils.Uuid.Encode($"{this.Path}{DELIMITER}{this.SiteId}", PURPOSE);
        }

        public string Path {
            get {
                return _path;
            }
            private set {
                if (string.IsNullOrEmpty(value) || value[0] != '/') {
                    throw new ApiArgumentException("path");
                }

                try {
                    this._path = FilesHelper.NormalizeVirtualPath(value);
                }
                catch (UriFormatException e) {
                    throw new ApiArgumentException("path", e);
                }
            }
        }

        public long SiteId { get; private set; }
        public string Uuid { get; private set; }
    }
}
