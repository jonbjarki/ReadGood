"use client"
import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination"
import { SearchPageParamsType } from "@/types/search-page-types";
import { assert } from "console";
import { useSearchParams } from "next/navigation";

type PaginationLinkType = {
  page: number;
  link: string;
}

function generatePageLink(page: number, params: URLSearchParams): string {
  const newParams = new URLSearchParams(params.toString());
  newParams.set("page", page.toString());
  return "/books/search?" + newParams.toString();
}

function getPaginationLinks(currentPage: number, params: URLSearchParams): PaginationLinkType[] {
  // NOTE: Books API does not provide a reliable "totalPages" value so we have to be greedy and just expect the client to handle non existent pages.
  const links: PaginationLinkType[] = [];
  // If we're on the first page, show the first 3 pages
  if (currentPage === 1) {
    for (let i = 1; i <= 3; i++) {
      links.push({ page: i, link: generatePageLink(i, params) });
    }
  }
  // If we're on a middle page, show the previous, current, and next pages
  else {
    links.push({ page: currentPage - 1, link: generatePageLink(currentPage - 1, params) });
    links.push({ page: currentPage, link: generatePageLink(currentPage, params) });
    links.push({ page: currentPage + 1, link: generatePageLink(currentPage + 1, params) });
  }

  return links;
}


type PaginationControlsProps = {
  parsedParams: SearchPageParamsType;
  itemsEmpty: boolean;
  hasNext: boolean;
  hasPrevious: boolean;
};



export default function PaginationControls({ parsedParams, itemsEmpty, hasNext, hasPrevious }: PaginationControlsProps) {
  const params = useSearchParams(); // Used for generating links, not for reading values since we already have parsedParams
  const { page } = parsedParams;
  const links = getPaginationLinks(page, params);

  if (itemsEmpty)
    return null; // Don't show pagination controls if there are no items to paginate

  return (
    <Pagination>
      <PaginationContent>
        <PaginationItem>
          <PaginationPrevious href={page > 1 ? generatePageLink(page - 1, params) : "#"} />
        </PaginationItem>
        {hasPrevious && (
          <PaginationItem>
            <PaginationLink href={generatePageLink(1, params)}>
              <PaginationEllipsis />
            </PaginationLink>
          </PaginationItem>
        )}
        {links.map(({ page, link }) => (
          <PaginationItem key={page}>
            <PaginationLink href={link} isActive={page === parsedParams.page}>
              {page}
            </PaginationLink>
          </PaginationItem>
        ))}

        {hasNext && (
          <PaginationItem>
            <PaginationEllipsis />
          </PaginationItem>
        )}

        <PaginationItem>
          <PaginationNext href={generatePageLink(page + 1, params)} />
        </PaginationItem>
      </PaginationContent>
    </Pagination>
  )
}
