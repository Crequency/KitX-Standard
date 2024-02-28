// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'operating_systems.dart';

// **************************************************************************
// BuiltValueGenerator
// **************************************************************************

const OperatingSystems _$unknown = const OperatingSystems._('unknown');
const OperatingSystems _$android = const OperatingSystems._('android');
const OperatingSystems _$browser = const OperatingSystems._('browser');
const OperatingSystems _$freeBSD = const OperatingSystems._('freeBSD');
const OperatingSystems _$iOS = const OperatingSystems._('iOS');
const OperatingSystems _$linux = const OperatingSystems._('linux');
const OperatingSystems _$macCatalyst = const OperatingSystems._('macCatalyst');
const OperatingSystems _$macOS = const OperatingSystems._('macOS');
const OperatingSystems _$tvOS = const OperatingSystems._('tvOS');
const OperatingSystems _$watchOS = const OperatingSystems._('watchOS');
const OperatingSystems _$windows = const OperatingSystems._('windows');
const OperatingSystems _$iot = const OperatingSystems._('iot');
const OperatingSystems _$appleVisionOS =
    const OperatingSystems._('appleVisionOS');

OperatingSystems _$operatingSystemsValueOf(String name) {
  switch (name) {
    case 'unknown':
      return _$unknown;
    case 'android':
      return _$android;
    case 'browser':
      return _$browser;
    case 'freeBSD':
      return _$freeBSD;
    case 'iOS':
      return _$iOS;
    case 'linux':
      return _$linux;
    case 'macCatalyst':
      return _$macCatalyst;
    case 'macOS':
      return _$macOS;
    case 'tvOS':
      return _$tvOS;
    case 'watchOS':
      return _$watchOS;
    case 'windows':
      return _$windows;
    case 'iot':
      return _$iot;
    case 'appleVisionOS':
      return _$appleVisionOS;
    default:
      throw new ArgumentError(name);
  }
}

final BuiltSet<OperatingSystems> _$operatingSystemsValues =
    new BuiltSet<OperatingSystems>(const <OperatingSystems>[
  _$unknown,
  _$android,
  _$browser,
  _$freeBSD,
  _$iOS,
  _$linux,
  _$macCatalyst,
  _$macOS,
  _$tvOS,
  _$watchOS,
  _$windows,
  _$iot,
  _$appleVisionOS,
]);

Serializer<OperatingSystems> _$operatingSystemsSerializer =
    new _$OperatingSystemsSerializer();

class _$OperatingSystemsSerializer
    implements PrimitiveSerializer<OperatingSystems> {
  static const Map<String, Object> _toWire = const <String, Object>{
    'unknown': 0,
    'android': 1,
    'browser': 2,
    'freeBSD': 3,
    'iOS': 4,
    'linux': 5,
    'macCatalyst': 6,
    'macOS': 7,
    'tvOS': 8,
    'watchOS': 9,
    'windows': 10,
    'iot': 11,
    'appleVisionOS': 12,
  };
  static const Map<Object, String> _fromWire = const <Object, String>{
    0: 'unknown',
    1: 'android',
    2: 'browser',
    3: 'freeBSD',
    4: 'iOS',
    5: 'linux',
    6: 'macCatalyst',
    7: 'macOS',
    8: 'tvOS',
    9: 'watchOS',
    10: 'windows',
    11: 'iot',
    12: 'appleVisionOS',
  };

  @override
  final Iterable<Type> types = const <Type>[OperatingSystems];
  @override
  final String wireName = 'OperatingSystems';

  @override
  Object serialize(Serializers serializers, OperatingSystems object,
          {FullType specifiedType = FullType.unspecified}) =>
      _toWire[object.name] ?? object.name;

  @override
  OperatingSystems deserialize(Serializers serializers, Object serialized,
          {FullType specifiedType = FullType.unspecified}) =>
      OperatingSystems.valueOf(
          _fromWire[serialized] ?? (serialized is String ? serialized : ''));
}

// ignore_for_file: deprecated_member_use_from_same_package,type=lint
