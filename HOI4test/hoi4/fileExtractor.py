import zipfile


def unzipfile(path, dest):
    with zipfile.ZipFile(path, 'r') as zip_ref:
        zip_ref.extractall(dest)
