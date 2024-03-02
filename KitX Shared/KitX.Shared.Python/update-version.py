import re

from datetime import datetime, timezone

file_pyproject = "pyproject.toml"
major_version = "3.24.10"
pattern = r"version = \"\d+.\d+.\d+.\d+\""


def calculateLatestVersionBuildNumber():
    current_utc = datetime.now(timezone.utc)

    specified_time = datetime(2020, 10, 7, tzinfo=timezone.utc)

    delta_days = (current_utc - specified_time).days

    build_version_code = delta_days % 65535 + 5602

    return str(build_version_code)


def updateVersion():
    ver = calculateLatestVersionBuildNumber()

    with open(file_pyproject, "r+") as file:
        data = file.read()
        data = re.sub(pattern, f"version = \"{major_version}.{ver}\"", data)
        file.seek(0)
        file.write(data)


if __name__ == "__main__":
    updateVersion()
