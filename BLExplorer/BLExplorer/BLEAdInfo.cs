using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;

namespace BLExplorer
{
    class BLEAdInfo
    {
        private BluetoothLEAdvertisement _advertisement;
        private BluetoothLEAdvertisementType _advertisementType;
        private System.UInt64 _bluetoothAddress;
        private System.Int16 _rawSignalStrengthInDBm;
        private DateTimeOffset _timestamp;

        public BluetoothLEAdvertisement Advertisement
        {
            get
            {
                return _advertisement;
            }
        }

        public BluetoothLEAdvertisementType AdvertisementType
        {
            get
            {
                return _advertisementType;
            }
        }

        public ulong BluetoothAddress
        {
            get
            {
                return _bluetoothAddress;
            }
        }

        public short RawSignalStrengthInDBm
        {
            get
            {
                return _rawSignalStrengthInDBm;
            }
        }

        public DateTimeOffset Timestamp
        {
            get
            {
                return _timestamp;
            }
        }

        public BLEAdInfo(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            _advertisement = args.Advertisement;
            _advertisementType = args.AdvertisementType;
            _bluetoothAddress = args.BluetoothAddress;
            _rawSignalStrengthInDBm = args.RawSignalStrengthInDBm;
            _timestamp = args.Timestamp;
        }

        public override String ToString()
        {
            return string.Concat(_bluetoothAddress.ToString()k);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            BLEAdInfo objAsPart = obj as BLEAdInfo;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

        public override int GetHashCode()
        {
            return (int)_bluetoothAddress;
        }

        public bool Equals(BLEAdInfo other)
        {
            if (other == null) return false;
            return (this._bluetoothAddress.Equals(other._bluetoothAddress));
        }

    }
}
