// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'device_info.dart';

// **************************************************************************
// BuiltValueGenerator
// **************************************************************************

Serializer<DeviceInfo> _$deviceInfoSerializer = new _$DeviceInfoSerializer();

class _$DeviceInfoSerializer implements StructuredSerializer<DeviceInfo> {
  @override
  final Iterable<Type> types = const [DeviceInfo, _$DeviceInfo];
  @override
  final String wireName = 'DeviceInfo';

  @override
  Iterable<Object?> serialize(Serializers serializers, DeviceInfo object,
      {FullType specifiedType = FullType.unspecified}) {
    final result = <Object?>[
      'Device',
      serializers.serialize(object.device,
          specifiedType: const FullType(DeviceLocator)),
      'DeviceOSVersion',
      serializers.serialize(object.deviceOSVersion,
          specifiedType: const FullType(String)),
      'PluginsServerPort',
      serializers.serialize(object.pluginsServerPort,
          specifiedType: const FullType(int)),
      'PluginsCount',
      serializers.serialize(object.pluginsCount,
          specifiedType: const FullType(int)),
      'SendTime',
      serializers.serialize(object.sendTime,
          specifiedType: const FullType(DateTime)),
      'IsMainDevice',
      serializers.serialize(object.isMainDevice,
          specifiedType: const FullType(bool)),
      'DevicesServerPort',
      serializers.serialize(object.devicesServerPort,
          specifiedType: const FullType(int)),
      'DevicesServerBuildTime',
      serializers.serialize(object.devicesServerBuildTime,
          specifiedType: const FullType(DateTime)),
      'DeviceOSType',
      serializers.serialize(object.deviceOSType,
          specifiedType: const FullType(OperatingSystems)),
    ];

    return result;
  }

  @override
  DeviceInfo deserialize(Serializers serializers, Iterable<Object?> serialized,
      {FullType specifiedType = FullType.unspecified}) {
    final result = new DeviceInfoBuilder();

    final iterator = serialized.iterator;
    while (iterator.moveNext()) {
      final key = iterator.current! as String;
      iterator.moveNext();
      final Object? value = iterator.current;
      switch (key) {
        case 'Device':
          result.device.replace(serializers.deserialize(value,
              specifiedType: const FullType(DeviceLocator))! as DeviceLocator);
          break;
        case 'DeviceOSVersion':
          result.deviceOSVersion = serializers.deserialize(value,
              specifiedType: const FullType(String))! as String;
          break;
        case 'PluginsServerPort':
          result.pluginsServerPort = serializers.deserialize(value,
              specifiedType: const FullType(int))! as int;
          break;
        case 'PluginsCount':
          result.pluginsCount = serializers.deserialize(value,
              specifiedType: const FullType(int))! as int;
          break;
        case 'SendTime':
          result.sendTime = serializers.deserialize(value,
              specifiedType: const FullType(DateTime))! as DateTime;
          break;
        case 'IsMainDevice':
          result.isMainDevice = serializers.deserialize(value,
              specifiedType: const FullType(bool))! as bool;
          break;
        case 'DevicesServerPort':
          result.devicesServerPort = serializers.deserialize(value,
              specifiedType: const FullType(int))! as int;
          break;
        case 'DevicesServerBuildTime':
          result.devicesServerBuildTime = serializers.deserialize(value,
              specifiedType: const FullType(DateTime))! as DateTime;
          break;
        case 'DeviceOSType':
          result.deviceOSType = serializers.deserialize(value,
                  specifiedType: const FullType(OperatingSystems))!
              as OperatingSystems;
          break;
      }
    }

    return result.build();
  }
}

class _$DeviceInfo extends DeviceInfo {
  @override
  final DeviceLocator device;
  @override
  final String deviceOSVersion;
  @override
  final int pluginsServerPort;
  @override
  final int pluginsCount;
  @override
  final DateTime sendTime;
  @override
  final bool isMainDevice;
  @override
  final int devicesServerPort;
  @override
  final DateTime devicesServerBuildTime;
  @override
  final OperatingSystems deviceOSType;

  factory _$DeviceInfo([void Function(DeviceInfoBuilder)? updates]) =>
      (new DeviceInfoBuilder()..update(updates))._build();

  _$DeviceInfo._(
      {required this.device,
      required this.deviceOSVersion,
      required this.pluginsServerPort,
      required this.pluginsCount,
      required this.sendTime,
      required this.isMainDevice,
      required this.devicesServerPort,
      required this.devicesServerBuildTime,
      required this.deviceOSType})
      : super._() {
    BuiltValueNullFieldError.checkNotNull(device, r'DeviceInfo', 'device');
    BuiltValueNullFieldError.checkNotNull(
        deviceOSVersion, r'DeviceInfo', 'deviceOSVersion');
    BuiltValueNullFieldError.checkNotNull(
        pluginsServerPort, r'DeviceInfo', 'pluginsServerPort');
    BuiltValueNullFieldError.checkNotNull(
        pluginsCount, r'DeviceInfo', 'pluginsCount');
    BuiltValueNullFieldError.checkNotNull(sendTime, r'DeviceInfo', 'sendTime');
    BuiltValueNullFieldError.checkNotNull(
        isMainDevice, r'DeviceInfo', 'isMainDevice');
    BuiltValueNullFieldError.checkNotNull(
        devicesServerPort, r'DeviceInfo', 'devicesServerPort');
    BuiltValueNullFieldError.checkNotNull(
        devicesServerBuildTime, r'DeviceInfo', 'devicesServerBuildTime');
    BuiltValueNullFieldError.checkNotNull(
        deviceOSType, r'DeviceInfo', 'deviceOSType');
  }

  @override
  DeviceInfo rebuild(void Function(DeviceInfoBuilder) updates) =>
      (toBuilder()..update(updates)).build();

  @override
  DeviceInfoBuilder toBuilder() => new DeviceInfoBuilder()..replace(this);

  @override
  bool operator ==(Object other) {
    if (identical(other, this)) return true;
    return other is DeviceInfo &&
        device == other.device &&
        deviceOSVersion == other.deviceOSVersion &&
        pluginsServerPort == other.pluginsServerPort &&
        pluginsCount == other.pluginsCount &&
        sendTime == other.sendTime &&
        isMainDevice == other.isMainDevice &&
        devicesServerPort == other.devicesServerPort &&
        devicesServerBuildTime == other.devicesServerBuildTime &&
        deviceOSType == other.deviceOSType;
  }

  @override
  int get hashCode {
    var _$hash = 0;
    _$hash = $jc(_$hash, device.hashCode);
    _$hash = $jc(_$hash, deviceOSVersion.hashCode);
    _$hash = $jc(_$hash, pluginsServerPort.hashCode);
    _$hash = $jc(_$hash, pluginsCount.hashCode);
    _$hash = $jc(_$hash, sendTime.hashCode);
    _$hash = $jc(_$hash, isMainDevice.hashCode);
    _$hash = $jc(_$hash, devicesServerPort.hashCode);
    _$hash = $jc(_$hash, devicesServerBuildTime.hashCode);
    _$hash = $jc(_$hash, deviceOSType.hashCode);
    _$hash = $jf(_$hash);
    return _$hash;
  }
}

class DeviceInfoBuilder implements Builder<DeviceInfo, DeviceInfoBuilder> {
  _$DeviceInfo? _$v;

  DeviceLocatorBuilder? _device;
  DeviceLocatorBuilder get device =>
      _$this._device ??= new DeviceLocatorBuilder();
  set device(DeviceLocatorBuilder? device) => _$this._device = device;

  String? _deviceOSVersion;
  String? get deviceOSVersion => _$this._deviceOSVersion;
  set deviceOSVersion(String? deviceOSVersion) =>
      _$this._deviceOSVersion = deviceOSVersion;

  int? _pluginsServerPort;
  int? get pluginsServerPort => _$this._pluginsServerPort;
  set pluginsServerPort(int? pluginsServerPort) =>
      _$this._pluginsServerPort = pluginsServerPort;

  int? _pluginsCount;
  int? get pluginsCount => _$this._pluginsCount;
  set pluginsCount(int? pluginsCount) => _$this._pluginsCount = pluginsCount;

  DateTime? _sendTime;
  DateTime? get sendTime => _$this._sendTime;
  set sendTime(DateTime? sendTime) => _$this._sendTime = sendTime;

  bool? _isMainDevice;
  bool? get isMainDevice => _$this._isMainDevice;
  set isMainDevice(bool? isMainDevice) => _$this._isMainDevice = isMainDevice;

  int? _devicesServerPort;
  int? get devicesServerPort => _$this._devicesServerPort;
  set devicesServerPort(int? devicesServerPort) =>
      _$this._devicesServerPort = devicesServerPort;

  DateTime? _devicesServerBuildTime;
  DateTime? get devicesServerBuildTime => _$this._devicesServerBuildTime;
  set devicesServerBuildTime(DateTime? devicesServerBuildTime) =>
      _$this._devicesServerBuildTime = devicesServerBuildTime;

  OperatingSystems? _deviceOSType;
  OperatingSystems? get deviceOSType => _$this._deviceOSType;
  set deviceOSType(OperatingSystems? deviceOSType) =>
      _$this._deviceOSType = deviceOSType;

  DeviceInfoBuilder();

  DeviceInfoBuilder get _$this {
    final $v = _$v;
    if ($v != null) {
      _device = $v.device.toBuilder();
      _deviceOSVersion = $v.deviceOSVersion;
      _pluginsServerPort = $v.pluginsServerPort;
      _pluginsCount = $v.pluginsCount;
      _sendTime = $v.sendTime;
      _isMainDevice = $v.isMainDevice;
      _devicesServerPort = $v.devicesServerPort;
      _devicesServerBuildTime = $v.devicesServerBuildTime;
      _deviceOSType = $v.deviceOSType;
      _$v = null;
    }
    return this;
  }

  @override
  void replace(DeviceInfo other) {
    ArgumentError.checkNotNull(other, 'other');
    _$v = other as _$DeviceInfo;
  }

  @override
  void update(void Function(DeviceInfoBuilder)? updates) {
    if (updates != null) updates(this);
  }

  @override
  DeviceInfo build() => _build();

  _$DeviceInfo _build() {
    _$DeviceInfo _$result;
    try {
      _$result = _$v ??
          new _$DeviceInfo._(
              device: device.build(),
              deviceOSVersion: BuiltValueNullFieldError.checkNotNull(
                  deviceOSVersion, r'DeviceInfo', 'deviceOSVersion'),
              pluginsServerPort: BuiltValueNullFieldError.checkNotNull(
                  pluginsServerPort, r'DeviceInfo', 'pluginsServerPort'),
              pluginsCount: BuiltValueNullFieldError.checkNotNull(
                  pluginsCount, r'DeviceInfo', 'pluginsCount'),
              sendTime: BuiltValueNullFieldError.checkNotNull(
                  sendTime, r'DeviceInfo', 'sendTime'),
              isMainDevice: BuiltValueNullFieldError.checkNotNull(
                  isMainDevice, r'DeviceInfo', 'isMainDevice'),
              devicesServerPort: BuiltValueNullFieldError.checkNotNull(
                  devicesServerPort, r'DeviceInfo', 'devicesServerPort'),
              devicesServerBuildTime: BuiltValueNullFieldError.checkNotNull(
                  devicesServerBuildTime, r'DeviceInfo', 'devicesServerBuildTime'),
              deviceOSType: BuiltValueNullFieldError.checkNotNull(
                  deviceOSType, r'DeviceInfo', 'deviceOSType'));
    } catch (_) {
      late String _$failedField;
      try {
        _$failedField = 'device';
        device.build();
      } catch (e) {
        throw new BuiltValueNestedFieldError(
            r'DeviceInfo', _$failedField, e.toString());
      }
      rethrow;
    }
    replace(_$result);
    return _$result;
  }
}

// ignore_for_file: deprecated_member_use_from_same_package,type=lint
