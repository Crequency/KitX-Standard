// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'device_locator.dart';

// **************************************************************************
// BuiltValueGenerator
// **************************************************************************

Serializer<DeviceLocator> _$deviceLocatorSerializer =
    new _$DeviceLocatorSerializer();

class _$DeviceLocatorSerializer implements StructuredSerializer<DeviceLocator> {
  @override
  final Iterable<Type> types = const [DeviceLocator, _$DeviceLocator];
  @override
  final String wireName = 'DeviceLocator';

  @override
  Iterable<Object?> serialize(Serializers serializers, DeviceLocator object,
      {FullType specifiedType = FullType.unspecified}) {
    final result = <Object?>[
      'DeviceName',
      serializers.serialize(object.deviceName,
          specifiedType: const FullType(String)),
      'IPv4',
      serializers.serialize(object.iPv4, specifiedType: const FullType(String)),
      'IPv6',
      serializers.serialize(object.iPv6, specifiedType: const FullType(String)),
      'MacAddress',
      serializers.serialize(object.macAddress,
          specifiedType: const FullType(String)),
    ];

    return result;
  }

  @override
  DeviceLocator deserialize(
      Serializers serializers, Iterable<Object?> serialized,
      {FullType specifiedType = FullType.unspecified}) {
    final result = new DeviceLocatorBuilder();

    final iterator = serialized.iterator;
    while (iterator.moveNext()) {
      final key = iterator.current! as String;
      iterator.moveNext();
      final Object? value = iterator.current;
      switch (key) {
        case 'DeviceName':
          result.deviceName = serializers.deserialize(value,
              specifiedType: const FullType(String))! as String;
          break;
        case 'IPv4':
          result.iPv4 = serializers.deserialize(value,
              specifiedType: const FullType(String))! as String;
          break;
        case 'IPv6':
          result.iPv6 = serializers.deserialize(value,
              specifiedType: const FullType(String))! as String;
          break;
        case 'MacAddress':
          result.macAddress = serializers.deserialize(value,
              specifiedType: const FullType(String))! as String;
          break;
      }
    }

    return result.build();
  }
}

class _$DeviceLocator extends DeviceLocator {
  @override
  final String deviceName;
  @override
  final String iPv4;
  @override
  final String iPv6;
  @override
  final String macAddress;

  factory _$DeviceLocator([void Function(DeviceLocatorBuilder)? updates]) =>
      (new DeviceLocatorBuilder()..update(updates))._build();

  _$DeviceLocator._(
      {required this.deviceName,
      required this.iPv4,
      required this.iPv6,
      required this.macAddress})
      : super._() {
    BuiltValueNullFieldError.checkNotNull(
        deviceName, r'DeviceLocator', 'deviceName');
    BuiltValueNullFieldError.checkNotNull(iPv4, r'DeviceLocator', 'iPv4');
    BuiltValueNullFieldError.checkNotNull(iPv6, r'DeviceLocator', 'iPv6');
    BuiltValueNullFieldError.checkNotNull(
        macAddress, r'DeviceLocator', 'macAddress');
  }

  @override
  DeviceLocator rebuild(void Function(DeviceLocatorBuilder) updates) =>
      (toBuilder()..update(updates)).build();

  @override
  DeviceLocatorBuilder toBuilder() => new DeviceLocatorBuilder()..replace(this);

  @override
  bool operator ==(Object other) {
    if (identical(other, this)) return true;
    return other is DeviceLocator &&
        deviceName == other.deviceName &&
        iPv4 == other.iPv4 &&
        iPv6 == other.iPv6 &&
        macAddress == other.macAddress;
  }

  @override
  int get hashCode {
    var _$hash = 0;
    _$hash = $jc(_$hash, deviceName.hashCode);
    _$hash = $jc(_$hash, iPv4.hashCode);
    _$hash = $jc(_$hash, iPv6.hashCode);
    _$hash = $jc(_$hash, macAddress.hashCode);
    _$hash = $jf(_$hash);
    return _$hash;
  }
}

class DeviceLocatorBuilder
    implements Builder<DeviceLocator, DeviceLocatorBuilder> {
  _$DeviceLocator? _$v;

  String? _deviceName;
  String? get deviceName => _$this._deviceName;
  set deviceName(String? deviceName) => _$this._deviceName = deviceName;

  String? _iPv4;
  String? get iPv4 => _$this._iPv4;
  set iPv4(String? iPv4) => _$this._iPv4 = iPv4;

  String? _iPv6;
  String? get iPv6 => _$this._iPv6;
  set iPv6(String? iPv6) => _$this._iPv6 = iPv6;

  String? _macAddress;
  String? get macAddress => _$this._macAddress;
  set macAddress(String? macAddress) => _$this._macAddress = macAddress;

  DeviceLocatorBuilder();

  DeviceLocatorBuilder get _$this {
    final $v = _$v;
    if ($v != null) {
      _deviceName = $v.deviceName;
      _iPv4 = $v.iPv4;
      _iPv6 = $v.iPv6;
      _macAddress = $v.macAddress;
      _$v = null;
    }
    return this;
  }

  @override
  void replace(DeviceLocator other) {
    ArgumentError.checkNotNull(other, 'other');
    _$v = other as _$DeviceLocator;
  }

  @override
  void update(void Function(DeviceLocatorBuilder)? updates) {
    if (updates != null) updates(this);
  }

  @override
  DeviceLocator build() => _build();

  _$DeviceLocator _build() {
    final _$result = _$v ??
        new _$DeviceLocator._(
            deviceName: BuiltValueNullFieldError.checkNotNull(
                deviceName, r'DeviceLocator', 'deviceName'),
            iPv4: BuiltValueNullFieldError.checkNotNull(
                iPv4, r'DeviceLocator', 'iPv4'),
            iPv6: BuiltValueNullFieldError.checkNotNull(
                iPv6, r'DeviceLocator', 'iPv6'),
            macAddress: BuiltValueNullFieldError.checkNotNull(
                macAddress, r'DeviceLocator', 'macAddress'));
    replace(_$result);
    return _$result;
  }
}

// ignore_for_file: deprecated_member_use_from_same_package,type=lint
