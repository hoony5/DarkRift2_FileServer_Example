﻿namespace Model;

public enum Tags
{
    REQUEST_CREATE_PARTY = 2000,
    RESPONSE_CREATE_PARTY,
    REQUEST_JOIN_PARTY,
    RESPONSE_JOIN_PARTY,
    REQUSET_JOIN_PARTY_WITH_PASSWORD,
    RESPONSE_JOIN_PARTY_WITH_PASSWORD,
    REQUSET_PARTY_LIST = 2100,
    RESPONSE_PARTY_LIST,
    REQUSET_EXIT_PARTY = 2200,
    RESPONSE_EXIT_PARTY,
    REQUSET_REMOVE_MEMBER,
    RESPONSE_REMOVE_MEMBER,
    REQUEST_DESTROY_PARTY,
    RESPONSE_DESTROY_PARTY,
    REQUEST_UPLOAD_VALIDATION = 3000,
    RESPONSE_UPLOAD_VALIDATION,
    REQUEST_UPLOAD_FILE,
    RESPONSE_UPLOAD_FILE,
    REQUEST_DOWNLOAD_VALIDATION = 3100,
    RESPONSE_DOWNLOAD_VALIDATION,
    REQUEST_DOWNLOAD_FILE,
    RESPONSE_DOWNLOAD_FILE,
    REQUEST_FILE_SEARCHING = 3200,
    RESPONSE_FILE_SEARCHING,
    REQUEST_CHECK_FILE_EXIST,
    RESPONSE_CHECK_FILE_EXIST,
    REQUEST_DELETE_ALL_FILES = 3300,
    RESPONSE_DELETE_ALL_FILES,
    REQUEST_ADVERTISE_UPLOAD_COMPLETION = 3400,
    RESPONSE_ADVERTISE_UPLOAD_COMPLETION,
    REQUEST_FILE_SHARE_START = 3500,
    RESPONSE_FILE_SHARE_START,
    REQUEST_UPDATE_FILE_SHARE_PROGRESS = 3600,
    RESPONSE_UPDATE_FILE_SHARE_PROGRESS,
    REQUEST_FILE_CURRENT_SHARED_PROGRESS,
    RESPONSE_FILE_CURRENT_SHARED_PROGRESS,
    REQUEST_FILE_SHARE_END = 3700,
    RESPONSE_FILE_SHARE_END,
}