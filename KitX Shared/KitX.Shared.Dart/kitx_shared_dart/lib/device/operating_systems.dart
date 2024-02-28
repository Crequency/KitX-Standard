import 'package:built_collection/built_collection.dart';
import 'package:built_value/built_value.dart';
import 'package:built_value/serializer.dart';

part 'operating_systems.g.dart';

class OperatingSystems extends EnumClass {
  @BuiltValueEnumConst(wireNumber: 0)
  static const OperatingSystems unknown = _$unknown;

  @BuiltValueEnumConst(wireNumber: 1)
  static const OperatingSystems android = _$android;

  @BuiltValueEnumConst(wireNumber: 2)
  static const OperatingSystems browser = _$browser;

  @BuiltValueEnumConst(wireNumber: 3)
  static const OperatingSystems freeBSD = _$freeBSD;

  @BuiltValueEnumConst(wireNumber: 4)
  static const OperatingSystems iOS = _$iOS;

  @BuiltValueEnumConst(wireNumber: 5)
  static const OperatingSystems linux = _$linux;

  @BuiltValueEnumConst(wireNumber: 6)
  static const OperatingSystems macCatalyst = _$macCatalyst;

  @BuiltValueEnumConst(wireNumber: 7)
  static const OperatingSystems macOS = _$macOS;

  @BuiltValueEnumConst(wireNumber: 8)
  static const OperatingSystems tvOS = _$tvOS;

  @BuiltValueEnumConst(wireNumber: 9)
  static const OperatingSystems watchOS = _$watchOS;

  @BuiltValueEnumConst(wireNumber: 10)
  static const OperatingSystems windows = _$windows;

  @BuiltValueEnumConst(wireNumber: 11)
  static const OperatingSystems iot = _$iot;

  @BuiltValueEnumConst(wireNumber: 12)
  static const OperatingSystems appleVisionOS = _$appleVisionOS;

  const OperatingSystems._(super.name);

  static BuiltSet<OperatingSystems> get values => _$operatingSystemsValues;

  static OperatingSystems valueOf(String name) => _$operatingSystemsValueOf(name);

  static Serializer<OperatingSystems> get serializer => _$operatingSystemsSerializer;
}
