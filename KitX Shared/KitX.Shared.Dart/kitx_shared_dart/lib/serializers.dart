library serializers;

import 'package:built_value/iso_8601_date_time_serializer.dart';
import 'package:built_value/serializer.dart';
import 'package:built_value/standard_json_plugin.dart';

import 'device/device_info.dart';
import 'device/device_locator.dart';

import 'device/operating_systems.dart';

part 'serializers.g.dart';

@SerializersFor([
  // Add the built values that require serialization
  DeviceInfo,
  DeviceLocator,
  OperatingSystems
])

/// Can add additional plugins that will serialize types like [DateTime]
///   - It is also possible to write your own Serializer plugins for type that
///   are not supported by default.
///   - For Example: https://github.com/google/built_value.dart/issues/543
///   implements [SerializerPlugin] and writes a serializer for Firebase
///   Datetime that converts TimeStamp or DateTime to integers.
final Serializers serializers = (_$serializers.toBuilder()
      ..addPlugin(StandardJsonPlugin())
      ..add(Iso8601DateTimeSerializer()))
    .build();
