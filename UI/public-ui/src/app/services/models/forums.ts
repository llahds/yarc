import { KeyValueModel } from "./common";

export interface Forum extends ForumEditModel {
    createdOn: Date;
    memberCount: number;
    isModerator: boolean;
    isOwner: boolean;
    hasJoined: boolean;
    isMuted: boolean;
}

export interface ForumEditModel {
    name: string; 
    description: string;
    slug: string;
    topics: KeyValueModel[];
    moderators: KeyValueModel[];
    isPrivate: boolean;
}

export interface ForumPostSettings extends ForumPostGuideLines {
    requiredTitleWords: string[];
    bannedTitleWords: string[];
    postTextBannedWords: string[];
    isDomainWhitelist: boolean;
    domains: string[];
}

export interface ForumPostGuideLines {
    guideLines: string;
}

export interface SimilarForum {
    id: number;
    name: string;
    memberCount: number;
}

export interface CanAccessForum {
    canAccessForum: boolean;
}
