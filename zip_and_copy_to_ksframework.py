# -*- coding:utf-8 -*-

'''

Desc：python3zip 

Time: 2020/7/27 17:58

Author: qingqing-zhao(569032731@qq.com)

'''

import os
import sys
import zipfile
import shutil

src_path = r"E:\Code\TableML\TableML\TableMLGUI\bin\Release"
dst_path = r"E:\Code\KSFramework_master_2018\KSFramework\Product\Tableml_GUI"

zip_name = ".\\tableml_gui.zip"

def zip_dir(dirname,zipfilename):
    filelist = []
    if os.path.isfile(dirname):
        filelist.append(dirname)
    else :
        for root, dirs, files in os.walk(dirname):
            for dir in dirs:
                filelist.append(os.path.join(root,dir))
            for name in files:
                filelist.append(os.path.join(root, name))

    zf = zipfile.ZipFile(zipfilename, "w", zipfile.zlib.DEFLATED)
    for tar in filelist:
        arcname = tar[len(dirname):]
        #参数二：zip文件夹内的名字，可以保留到去掉根目录的层级
        zf.write(tar,arcname)
    zf.close()

if __name__ == "__main__":

    try:
        if len(sys.argv) >= 2:
            src_path = sys.argv[1];
            zip_name = sys.argv[2];

        if not os.path.exists(src_path):
            os.makedirs(src_path)
            print("not exist path,create", src_path)

        zip_path = os.path.abspath(os.path.dirname(zip_name))
        if not os.path.exists(zip_path):
            os.makedirs(zip_path)
            print("not exist path,create", zip_path)

        zip_dir(src_path,zip_name)

        #目录拷贝
        if os.path.exists(dst_path):
            shutil.rmtree(dst_path)
            print("exist path,delete", dst_path)
        shutil.copytree(src_path, dst_path)

    except Exception as ex:
        print(ex)
    finally:
        print("zip finish")
        input("Press <Enter>")