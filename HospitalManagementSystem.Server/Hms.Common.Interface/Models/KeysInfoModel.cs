﻿namespace Hms.Common.Interface.Models
{
    using System;

    public class KeysInfoModel
    {
        public byte[] PublicKey { get; set; }

        public byte[] RoundKey { get; set; }

        public int RoundKeySentTimes { get; set; }

        public DateTime GeneratedTimeUtc { get; set; }
    }
}