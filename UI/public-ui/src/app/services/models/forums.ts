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