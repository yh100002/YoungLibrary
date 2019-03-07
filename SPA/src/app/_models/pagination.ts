export interface Pagination {
    count:number;
    from: number;
    index: number;
    size: number;
    pages: number;
    hasNext: boolean;
    hasPrevious: boolean;
}

export class PaginatedResult<T> {
    items: T;    
    count:number;
    from: number;
    index: number;
    size: number;
    pages: number;
    hasNext: boolean;
    hasPrevious: boolean;
}