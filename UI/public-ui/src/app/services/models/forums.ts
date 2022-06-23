import { KeyValueModel } from "./common";

export interface Forum extends ForumEditModel {
    createdOn: Date;
}

export interface ForumEditModel {
    name: string; 
    description: string;
    slug: string;
    topics: KeyValueModel[]
    moderators: KeyValueModel[]
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

