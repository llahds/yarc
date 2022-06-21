export interface Register {
    email: string;
    userName: string;
    password: string;
    confirmPassword: string;
}

export interface AuthenticationToken {
    token: string;
    userName: string;
}

export interface UserSettings {
    displayName: string;
    about: string;
}

export interface ChangeUserName {
    userName: string;
    password: string;
}

export interface ChangeEmail {
    email: string;
    password: string;
}

export interface ChangePassword {
    oldPassword: string;
    password: string;
    confirmPassword: string;
}

export const FORUM_STATUS_BANNED = 0;
export const FORUM_STATUS_MUTED = 1;
export const FORUM_STATUS_APPROVED = 2;

export interface UserInfo {
    id: number;
    userName: string;
}