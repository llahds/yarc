export interface KeyValueModel {
    id: number;
    name: string;
}

export interface PostedBy  {
    avatarId: number;
    id: number;
    name: string;
}

export interface Id {
    id: number;
}

export interface ListResult<TEntity> {
    list: TEntity[];
    sortBy: string;
    total: number;
    pageSize: number;
}