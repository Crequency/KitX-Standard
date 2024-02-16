import 'dart:convert';

import 'package:built_value/built_value.dart';
import 'package:built_value/serializer.dart';

import 'device_locator.dart';
import 'operating_systems.dart';

import '../serializers.dart';

part 'device_info.g.dart';

abstract class DeviceInfo implements Built<DeviceInfo, DeviceInfoBuilder> {
  @BuiltValueField(wireName: 'Device')
  DeviceLocator get device;

  @BuiltValueField(wireName: 'DeviceOSVersion')
  String get deviceOSVersion;

  @BuiltValueField(wireName: 'PluginsServerPort')
  int get pluginsServerPort;

  @BuiltValueField(wireName: 'PluginsCount')
  int get pluginsCount;

  @BuiltValueField(wireName: 'SendTime')
  DateTime get sendTime;

  @BuiltValueField(wireName: 'IsMainDevice')
  bool get isMainDevice;

  @BuiltValueField(wireName: 'DevicesServerPort')
  int get devicesServerPort;

  @BuiltValueField(wireName: 'DevicesServerBuildTime')
  DateTime get devicesServerBuildTime;

  @BuiltValueField(wireName: 'DeviceOSType')
  OperatingSystems get deviceOSType;

  DeviceInfo._();

  factory DeviceInfo([void Function(DeviceInfoBuilder) updates]) = _$DeviceInfo;

  Object? toJson() {
    return serializers.serializeWith(DeviceInfo.serializer, this);
  }

  @override
  String toString() {
    return json.encode(serializers.serializeWith(DeviceInfo.serializer, this));
  }

  static DeviceInfo? fromString(String jsonString) {
    DeviceInfo? result = serializers.deserializeWith(DeviceInfo.serializer, json.decode(jsonString));
    return result;
  }

  static DeviceInfo? fromJson(Map<String, dynamic> json) {
    return serializers.deserializeWith(DeviceInfo.serializer, json);
  }

  static Serializer<DeviceInfo> get serializer => _$deviceInfoSerializer;
}
